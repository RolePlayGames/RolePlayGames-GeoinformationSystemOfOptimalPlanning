import { useNavigate } from "react-router-dom";
import { InputField, SelectField } from "../common/inputs";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { FilmRecipe, updateFilmRecipe, createFilmRecipe, deleteFilmRecipe, AvaliableFilmType } from "./filmRecipesClient";
import { convertToNumber } from "../utils/number-converters/numberConverter";
import { useItemField, useItemFieldWithValidation } from "../common/useItemField";
import { validateName, validateThickness, validateProductionSpeed, validateMaterialCost, validateNozzle, validateCalibration, validateCoolingLip } from "./validations";
import { IClientError } from "../common/clients/clientError";

type FilmRecipeElementProps = {
    id: number,
    item: FilmRecipe,
    apiPath: string,
	filmTypes: AvaliableFilmType[],
}

export const FilmRecipeElement = ({ id, item, apiPath, filmTypes }: FilmRecipeElementProps) => {
	const [name, setName, nameError, setNameError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.name, validateName);
	const [filmTypeID, setFilmTypeId] = useItemField<FilmRecipe, number | undefined>(item, item => filmTypes.findIndex(x => x.id === item.filmTypeID) > -1 ? item.filmTypeID : filmTypes[0].id);
	const [thickness, setThickness, thicknessError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.thickness.toString(), validateThickness);
	const [productionSpeed, setProductionSpeed, productionSpeedError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.productionSpeed.toString(), validateProductionSpeed);
	const [materialCost, setMaterialCost, materialCostError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.materialCost.toString(), validateMaterialCost);
	const [nozzle, setNozzle, nozzleError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.nozzle.toString(), validateNozzle);
	const [calibration, setCalibration, calibrationError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.calibration.toString(), validateCalibration);
	const [coolingLip, setCoolingLip, coolingLipError] = useItemFieldWithValidation<FilmRecipe, string>(item, x => x.coolingLip.toString(), validateCoolingLip);

	const navigate = useNavigate();

	const onUpdate = async (item: FilmRecipe) => {
		try {
			await updateFilmRecipe(id, item);
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmRecipeNameAlreadyExistsException')
				setNameError('Указанное название уже используется в системе');
		}
	};

	const onCreate = async (item: FilmRecipe) => {
		try {
			await createFilmRecipe(item);
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmRecipeArticleAlreadyExistsException')
				setNameError('Указанное название уже используется в системе');
		}
	};

	const onSave = () => {
		const thicknessNumber = convertToNumber(thickness);
		const productionSpeedNumber = convertToNumber(productionSpeed);
		const materialCostNumber = convertToNumber(materialCost);
		const nozzleNumber = convertToNumber(nozzle);
		const calibrationNumber = convertToNumber(calibration);
		const coolingLipNumber = convertToNumber(coolingLip);

		if (thicknessNumber !== undefined
			&& productionSpeedNumber !== undefined
			&& materialCostNumber !== undefined
			&& nozzleNumber !== undefined
			&& calibrationNumber !== undefined
			&& coolingLipNumber !== undefined
			&& filmTypeID !== undefined)
		{
			const item = {
				name: name,
				filmTypeID: filmTypeID,
				thickness: thicknessNumber,
				productionSpeed: productionSpeedNumber,
				materialCost: materialCostNumber,
				nozzle: nozzleNumber,
				calibration: calibrationNumber,
				coolingLip: coolingLipNumber,
			};

			if (id > 0) 
				return onUpdate(item);
			else 
				return onCreate(item);
		}
		
		return Promise.resolve();
	};

	const onDelete = async () => {
		if (id > 0) {
			await deleteFilmRecipe(id);
			navigate(apiPath);
		}
	};

	return(
		<ElementContainer>
			<HeaderLabel>Рецепт {item.name}</HeaderLabel>
			<ActionsBar>
				<SaveButton
					onClick={onSave}
					disabled={
						!!nameError
						&& !!thicknessError
						&& !!productionSpeedError
						&& !!materialCostError
						&& !!nozzleError
						&& !!calibrationError
						&& !!coolingLipError
						&& filmTypeID !== undefined
					}
				/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<InputField
				label='Название'
				value={name}
				onChange={setName}
				errorText={nameError}/>
			<SelectField
				label='Тип пленки'
				items={filmTypes}
				onItemChanged={setFilmTypeId}
				variant='standard'
				value={filmTypeID}
			/>
			<InputField
				label='Толщина'
				value={thickness}
				onChange={setThickness}
				errorText={thicknessError}/>
			<InputField
				label='Скорость производства'
				value={productionSpeed}
				onChange={setProductionSpeed}
				errorText={productionSpeedError}/>
			<InputField
				label='Стоимость материала'
				value={materialCost}
				onChange={setMaterialCost}
				errorText={materialCostError}/>
			<InputField
				label='Размер сопла'
				value={nozzle}
				onChange={setNozzle}
				errorText={nozzleError}/>
			<InputField
				label='Калибровка'
				value={calibration}
				onChange={setCalibration}
				errorText={calibrationError}/>
			<InputField
				label='Расстояние между валиками'
				value={coolingLip}
				onChange={setCoolingLip}
				errorText={coolingLipError}/>
		</ElementContainer>
	);
}
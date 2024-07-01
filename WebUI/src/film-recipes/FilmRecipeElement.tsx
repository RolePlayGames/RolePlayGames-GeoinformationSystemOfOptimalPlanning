import { ReactNode, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { InputField, SelectField } from "../common/inputs/inputs";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { FilmRecipe, updateFilmRecipe, IClientError, createFilmRecipe, deleteFilmRecipe, AvaliableFilmType } from "./filmRecipesClient";
import { convertToNumber } from "../utils/number-converters/numberConverter";
import { MenuItem, SelectChangeEvent } from "@mui/material";

const validateName = (name: string) => {
	if (name.length == 0)
		return 'Заполните название';

	if (name.length > 20)
		return 'Название не должно превышать 20 символов';

	return undefined;
}

const validateThickness = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Толщина должна быть положительной';

	if (number > 300)
		return 'Толщина не должна быть больше 300';

	return undefined;
}

const validateProductionSpeed = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Скорость производства должна быть положительной';

	if (number > 300)
		return 'Скорость производства не должна быть больше 300';

	return undefined;
}

const validateMaterialCost = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Стоимость материала должна быть положительной';

	return undefined;
}

const validateNozzle = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Размер сопла должен быть положительной';

	if (number > 300)
		return 'Размер сопла не должен быть больше 300';

	return undefined;
}

const validateCalibration = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Калибровка должна быть положительной';

	if (number > 300)
		return 'Калибровка не должна быть больше 300';

	return undefined;
}

const validateCoolingLip = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Расстояние между валиками должно быть положительной';

	if (number > 1000)
		return 'Расстояние между валиками не должно быть больше 1000';

	return undefined;
}

type FilmRecipeElementProps = {
    id: number,
    item: FilmRecipe,
    apiPath: string,
	filmTypes: AvaliableFilmType[],
}

export const FilmRecipeElement = ({ id, item, apiPath, filmTypes }: FilmRecipeElementProps) => {
	const [name, setName] = useState(item.name);
	const [nameError, setNameError] = useState<string>();
	const [filmTypeID, setFilmTypeId] = useState(filmTypes.findIndex(x => x.id === item.filmTypeID) > -1 ? item.filmTypeID : filmTypes[0].id);
	const [thickness, setThickness] = useState(item.thickness.toString());
	const [thicknessError, setThicknessError] = useState<string>();
	const [productionSpeed, setProductionSpeed] = useState(item.productionSpeed.toString());
	const [productionSpeedError, setProductionSpeedError] = useState<string>();
	const [materialCost, setMaterialCost] = useState(item.materialCost.toString());
	const [materialCostError, setMaterialCostError] = useState<string>();
	const [nozzle, setNozzle] = useState(item.nozzle.toString());
	const [nozzleError, setNozzleError] = useState<string>();
	const [calibration, setCalibration] = useState(item.calibration.toString());
	const [calibrationError, setCalibrationError] = useState<string>();
	const [coolingLip, setCoolingLip] = useState(item.coolingLip.toString());
	const [coolingLipError, setCoolingLipError] = useState<string>();

	const navigate = useNavigate();
    
	useEffect(() => {
		setName(item.name);
		setFilmTypeId(filmTypes.findIndex(x => x.id === item.filmTypeID) > -1 ? item.filmTypeID : filmTypes[0].id);
		setThickness(item.thickness.toString());
		setProductionSpeed(item.productionSpeed.toString());
		setMaterialCost(item.materialCost.toString());
		setNozzle(item.nozzle.toString());
		setCalibration(item.calibration.toString());
		setCoolingLip(item.coolingLip.toString());
	}, [item]);
    
	useEffect(() => {
		setNameError(validateName(name));
	}, [name]);
    
	useEffect(() => {
		setThicknessError(validateThickness(thickness));
	}, [thickness]);
    
	useEffect(() => {
		setProductionSpeedError(validateProductionSpeed(productionSpeed));
	}, [productionSpeed]);
    
	useEffect(() => {
		setMaterialCostError(validateMaterialCost(materialCost));
	}, [materialCost]);
    
	useEffect(() => {
		setNozzleError(validateNozzle(nozzle));
	}, [nozzle]);
    
	useEffect(() => {
		setCalibrationError(validateCalibration(calibration));
	}, [calibration]);
    
	useEffect(() => {
		setCoolingLipError(validateCoolingLip(coolingLip));
	}, [coolingLip]);

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

		if (thicknessNumber && productionSpeedNumber && materialCostNumber && nozzleNumber && calibrationNumber && coolingLipNumber)
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

	const changeFilmType = (event: SelectChangeEvent<unknown>, child: ReactNode) => {
		const newFilmTypeId = Number(event?.target?.value);



		setFilmTypeId(newFilmTypeId);
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
				onChange={changeFilmType}
				variant='standard'
				value={filmTypeID.toString()}
				defaultValue={filmTypes.length === 0 ? undefined : filmTypes[0].id.toString()}
			>
				{filmTypes.map((filmType) => 
					<MenuItem 
						key={filmType.id}
						value={filmType.id}
					>
						{filmType.name}
					</MenuItem>
				)}
			</SelectField>
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
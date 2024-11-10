import { useNavigate } from "react-router-dom";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { createProductionLine, deleteProductionLine, ProductionLine, updateProductionLine } from "./productionLinesClient";
import { convertToNumber } from "../utils/number-converters/numberConverter";
import { useItemField, useItemFieldWithValidation } from "../common/useItemField";
import dayjs, { Dayjs } from "dayjs";
import { IClientError } from "../common/clients/clientError";
import { InputField } from "../common/inputs";
import { validateHourCost, validateMaxProductionSpeed, validateMaxThickness, validateMaxWidth, validateMinThickness, validateMinWidth, validateName, validateWidthChangeConsumption } from "./validations";
import { TimeWithoutSelectField } from "../common/inputs/TimeField";

const convertFromTimeSpan = (value: string) => {
	// Assuming timespanString is in the format "HH:mm:ss" (e.g., "02:30:15")
	const [hours, minutes, seconds] = value.split(':').map(Number);
	return dayjs().hour(hours).minute(minutes).second(seconds);
}

const convertToTimeSpan = (value: Dayjs) => {
	const hours = value.hour().toString().padStart(2, '0');
	const minutes = value.minute().toString().padStart(2, '0');
	const seconds = value.second().toString().padStart(2, '0');
	return `${hours}:${minutes}:${seconds}`;
}

type ProductionLineElementProps = {
    id: number,
    item: ProductionLine,
    apiPath: string,
}

export const ProductionLineElement = ({ id, item, apiPath }: ProductionLineElementProps) => {
	const [name, setName, nameError, setNameError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.name, validateName);
	const [hourCost, setHourCost, hourCostError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.hourCost.toString(), validateHourCost);
	const [maxProductionSpeed, setMaxProductionSpeed, maxProductionSpeedError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.maxProductionSpeed.toString(), validateMaxProductionSpeed);
	const [minWidth, setMinWidth, minWidthError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthMin.toString(), validateMinWidth);
	const [maxWidth, setMaxWidth, maxWidthError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthMax.toString(), x => validateMaxWidth(x, minWidth));
	const [minThickness, setMinThickness, minThicknessError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessMin.toString(), validateMinThickness);
	const [maxThickness, setMaxThickness, maxThicknessError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessMax.toString(), x => validateMaxThickness(x, minThickness));
	const [widthChangeTime, setWidthChangeTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.widthChangeTime));
	const [widthChangeConsumption, setWidthChangeConsumption, widthChangeConsumptionError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthChangeConsumption.toString(), validateWidthChangeConsumption);
	const [thicknessChangeTime, setThicknessChangeTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.thicknessChangeTime));
	const [thicknessChangeConsumption, setThicknessChangeConsumption, thicknessChangeConsumptionError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessChangeConsumption.toString(), validateWidthChangeConsumption);
	const [setupTime, setSetupTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.setupTime));

	const navigate = useNavigate();

	const onUpdate = async (item: ProductionLine) => {
		try {
			await updateProductionLine(id, item);
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionLineNameAlreadyExistsException')
				setNameError('Указанное название уже используется в системе');
		}
	};

	const onCreate = async (item: ProductionLine) => {
		try {
			await createProductionLine(item);
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionLineNameAlreadyExistsException')
				setNameError('Указанное название уже используется в системе');
		}
	};

	const onSave = () => {
		const hourCostNumber = convertToNumber(hourCost);
		const maxProductionSpeedNumber = convertToNumber(maxProductionSpeed);
		const minWidthNumber = convertToNumber(minWidth);
		const maxWidthNumber = convertToNumber(maxWidth);
		const minThicknessNumber = convertToNumber(minThickness);
		const maxThicknessNumber = convertToNumber(maxThickness);
		const widthChangeConsumptionNumber = convertToNumber(widthChangeConsumption);
		const thicknessChangeConsumptionNumber = convertToNumber(thicknessChangeConsumption);

		if (hourCostNumber !== undefined
			&& maxProductionSpeedNumber !== undefined
			&& minWidthNumber !== undefined
			&& maxWidthNumber !== undefined
			&& minThicknessNumber !== undefined
			&& maxThicknessNumber !== undefined
            && widthChangeTime !== null
			&& widthChangeConsumptionNumber !== undefined
			&& thicknessChangeTime !== null
			&& thicknessChangeConsumptionNumber !== undefined
			&& setupTime !== null)
		{
			const item = {
				name: name,
				hourCost: hourCostNumber,
				maxProductionSpeed: maxProductionSpeedNumber,
				widthMin: minWidthNumber,
				widthMax: maxWidthNumber,
				thicknessMin: minThicknessNumber,
				thicknessMax: maxThicknessNumber,
				thicknessChangeTime: convertToTimeSpan(thicknessChangeTime),
				thicknessChangeConsumption: widthChangeConsumptionNumber,
				widthChangeTime: convertToTimeSpan(widthChangeTime),
				widthChangeConsumption: thicknessChangeConsumptionNumber,
				setupTime: convertToTimeSpan(setupTime),
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
			await deleteProductionLine(id);
			navigate(apiPath);
		}
	};

	return(
		<ElementContainer>
			<HeaderLabel>Производственная линия {item.name}</HeaderLabel>
			<ActionsBar>
				<SaveButton
					onClick={onSave}
					disabled={
						!!nameError
						&& !!hourCostError
						&& !!maxProductionSpeedError
						&& !!minWidthError
						&& !!maxWidthError
						&& !!minThicknessError
						&& !!maxThicknessError
						&& widthChangeTime === null
						&& !!widthChangeConsumptionError
						&& thicknessChangeTime === null
						&& !!thicknessChangeConsumptionError
						&& setupTime === null
					}
				/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<InputField
				label='Название'
				value={name}
				onChange={setName}
				errorText={nameError}/>
			<InputField
				label='Стоимость часа работы, у.е./ч.'
				value={hourCost}
				onChange={setHourCost}
				errorText={hourCostError}/>
			<InputField
				label='Максимальная скорость производства, у.е./час'
				value={maxProductionSpeed}
				onChange={setMaxProductionSpeed}
				errorText={maxProductionSpeedError}/>
			<InputField
				label='Минимальная ширина рулона, мм'
				value={minWidth}
				onChange={setMinWidth}
				errorText={minWidthError}/>
			<InputField
				label='Максимальная ширина рулона, мм'
				value={maxWidth}
				onChange={setMaxWidth}
				errorText={maxWidthError}/>
			<InputField
				label='Минимальная толщина рулона, мкм'
				value={minThickness}
				onChange={setMinThickness}
				errorText={minThicknessError}/>
			<InputField
				label='Максимальная толщина рулона, мкм'
				value={maxThickness}
				onChange={setMaxThickness}
				errorText={maxThicknessError}/>
			<TimeWithoutSelectField
				label='Время перенастройки по ширине'
				value={widthChangeTime}
				onChange={setWidthChangeTime}/>
			<InputField
				label='Расход при перенастройке по ширине, кг/час'
				value={widthChangeConsumption}
				onChange={setWidthChangeConsumption}
				errorText={widthChangeConsumptionError}/>
			<TimeWithoutSelectField
				label='Время перенастройки по толщине'
				value={thicknessChangeTime}
				onChange={setThicknessChangeTime}/>
			<InputField
				label='Расход при перенастройке по толщине, кг/час'
				value={thicknessChangeConsumption}
				onChange={setThicknessChangeConsumption}
				errorText={thicknessChangeConsumptionError}/>
			<TimeWithoutSelectField
				label='Время прогрева'
				value={setupTime}
				onChange={setSetupTime}/>
		</ElementContainer>
	);
}
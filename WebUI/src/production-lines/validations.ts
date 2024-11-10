import { convertToNumber } from "../utils/number-converters/numberConverter";

export const validateName = (name: string) => {
	if (name.length == 0)
		return 'Заполните название';

	if (name.length < 3)
		return 'Название не должно быть короче 3 символов';

	if (name.length > 20)
		return 'Название не должно превышать 20 символов';

	return undefined;
}

export const validateHourCost = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Стоимость часа работы должна быть неотрицательной';

	return undefined;
}

export const validateMaxProductionSpeed = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Максимальняая скорость производства должна быть неотрицательной';

	return undefined;
}

export const validateMinWidth = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Минимальная ширина рулона должна быть неотрицательной';

	return undefined;
}

export const validateMaxWidth = (maxWidth: string, minWidth: string) => {
	const maxWidthNumber = convertToNumber(maxWidth);
	const minWidthNumber = convertToNumber(minWidth);
	
	if (maxWidthNumber === undefined)
		return 'Введите число';

	if (maxWidthNumber < 0)
		return 'Максимальная ширина рулона должна быть неотрицательной';

	if (minWidthNumber !== undefined && maxWidthNumber < minWidthNumber)
		return 'Максимальная ширина рулона не должна быть меньше минимальной ширины';

	return undefined;
}

export const validateMinThickness = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Минимальная ширина рулона должна быть неотрицательной';

	return undefined;
}

export const validateMaxThickness = (maxThickness: string, minThickness: string) => {
	const maxThicknessNumber = convertToNumber(maxThickness);
	const minThicknessNumber = convertToNumber(minThickness);
	
	if (maxThicknessNumber === undefined)
		return 'Введите число';

	if (maxThicknessNumber < 0)
		return 'Максимальная толщина материала должна быть неотрицательной';

	if (minThicknessNumber !== undefined && maxThicknessNumber < minThicknessNumber)
		return 'Максимальная толщина материала не должна быть меньше минимальной ширины';

	return undefined;
}

export const validateWidthChangeConsumption = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Расход материала при смене ширины должен быть неотрицательным';

	return undefined;
}

export const validateThicknessChangeConsumption = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';
	
	if (number < 0)
		return 'Расход материала при смене толщины должен быть неотрицательным';

	return undefined;
}
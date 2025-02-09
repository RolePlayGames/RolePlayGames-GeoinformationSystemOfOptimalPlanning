import { convertToNumber } from "../utils/number-converters/numberConverter";

export const validateName = (name: string) => {
	if (name.length == 0)
		return 'Заполните название';

	if (name.length > 20)
		return 'Название не должно превышать 20 символов';

	return undefined;
}

export const validateThickness = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Толщина должна быть положительной';

	if (number > 300)
		return 'Толщина не должна быть больше 300';

	return undefined;
}

export const validateProductionSpeed = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Скорость производства должна быть положительной';

	if (number > 300)
		return 'Скорость производства не должна быть больше 300';

	return undefined;
}

export const validateMaterialCost = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Стоимость материала должна быть положительной';

	return undefined;
}

export const validateNozzle = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Размер сопла должен быть положительной';

	if (number > 300)
		return 'Размер сопла не должен быть больше 300';

	return undefined;
}

export const validateCalibration = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Калибровка должна быть положительной';

	if (number > 600)
		return 'Калибровка не должна быть больше 600';

	return undefined;
}

export const validateCoolingLip = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number <= 0)
		return 'Расстояние между валиками должно быть положительной';

	if (number > 1000)
		return 'Расстояние между валиками не должно быть больше 1000';

	return undefined;
}
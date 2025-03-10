import { convertToInt, convertToNumber } from "../utils/number-converters/numberConverter";

export const validateNumber = (name: string) => {
	if (name.length == 0)
		return 'Заполните номер';

	if (name.length > 20)
		return 'Номер не должен превышать 20 символов';

	return undefined;
}

export const validateWidth = (value: string) => {
	const number = convertToInt(value);

	if (number === undefined)
		return 'Введите целое число';

	if (number <= 0)
		return 'Ширина должна быть положительной';

	if (number > 10000)
		return 'Ширина не должна быть больше 10000';

	return undefined;
}

export const validateQuantityInRunningMeter = (value: string) => {
	const number = convertToInt(value);

	if (number === undefined)
		return 'Введите целое число';

	if (number <= 0)
		return 'Количество в погонном метре должно быть положительным';

	return undefined;
}

export const validateFinishedGoods = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Количество выходного материала должно быть неотрицательным';

	return undefined;
}

export const validateWaste = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Расход должен быть неотрицательным';

	return undefined;
}

export const validateRollsCount = (value: string) => {
	const number = convertToInt(value);

	if (number === undefined)
		return 'Введите целое число';

	if (number <= 0)
		return 'Количество рулонов должен быть положительным';

	return undefined;
}

export const validatePriceOverdue = (value: string) => {
	const number = convertToNumber(value);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Штраф за нарушение сроков должен быть не отрицательным';

	return undefined;
}
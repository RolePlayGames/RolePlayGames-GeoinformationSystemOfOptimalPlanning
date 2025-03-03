import { convertToInt, convertToNumber } from "../utils/number-converters/numberConverter";

export const validateMaxIterationsCount = (value: string) => {
	if (value === '')
		return '';

	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите целое положительное число';

	return '';
}

export const validateGenerationsCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное число';

	return '';
}

export const validateMutationCoefficient = (value: string) => {
	const num = convertToNumber(value);

	if (num === undefined || num <= 0 || num >= 1)
		return 'Коэффициент должен быть в пределах от 0 до 1';

	return '';
}

export const validateMutationSelectionCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное целое число';

	return '';
}

export const validateCrossoverSelectionCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное целое число';

	return '';
}

export const validateIndividualsInPopulationCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное целое число';

	return '';
}

export const validateCrossoverPointsCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное целое число';

	return '';
}

export const validatePointedMutationProbability = (value: string) => {
	const num = convertToNumber(value);

	if (num === undefined || num < 0 || num > 1)
		return 'Вероятность должна быть в пределах от 0 до 1';

	return '';
}

export const validateStartPopulationsCount = (value: string) => {
	const num = convertToInt(value);

	if (num === undefined || num < 1)
		return 'Введите положительное целое число';

	return '';
}
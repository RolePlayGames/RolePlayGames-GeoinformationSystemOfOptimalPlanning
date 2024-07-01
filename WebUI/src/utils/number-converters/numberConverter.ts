/**
 * Converts string to number
 * @param value string value to convert
 * @returns number as convertion result or undefined if failure
 */
export const convertToNumber = (value: string): number | undefined => {
    
	if (value.includes(','))
		value = value.replaceAll(',', '.');

	const number = Number(value);

	if (typeof number === 'number' && !isNaN(number))
		return number;

	return undefined;
}
/**
 * Get variable name.
 * @param {*} varObj - Variable to get name (should be an object in curve braces like { variable }).
 * @returns Name of variable.
 */
export const nameof = (varObj: object): string => Object.keys(varObj)[0];

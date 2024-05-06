import { nameof } from './nameof';

it('nameof returns variable name', () => {
	// arrange
	const variable = 3;

	// act
	const result: string = nameof({ variable });

	// assert
	expect(result).toBe('variable');
});

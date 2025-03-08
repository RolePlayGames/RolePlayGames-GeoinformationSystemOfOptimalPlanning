import { Dayjs } from "dayjs";
import { useEffect } from "react";
import { InputField } from "../../common/inputs";
import { TimeWithoutSelectField } from "../../common/inputs/TimeField";
import { useItemFieldWithValidation, useItemField } from "../../common/useItemField";
import { validateCalibration } from "../../film-recipes/validations";
import { convertToNumber } from "../../utils/number-converters/numberConverter";
import { CalibratoinChangeRule } from "../productionLinesClient";
import { validateWidthChangeConsumption } from "../validations";
import { convertFromTimeSpan, convertToTimeSpan } from "../timespanConverter";
import { DeleteButton } from "../../common/elementControls";
import { BigFieldContainer, DeleteButtonContainer, FieldContainer, RuleContainer } from "./RuleComponents";

type CalibratoinChangeRuleComponentProps = {
	rule: CalibratoinChangeRule,
	index: number,
	onDelete: (index: number) => void,
	rules: CalibratoinChangeRule[],
	onValueChanged: () => void,
}

export const CalibratoinChangeRuleComponent = ({ rule, index, onDelete, rules, onValueChanged }: CalibratoinChangeRuleComponentProps) => {
	const [calibrationTo, setCalibrationTo, calibrationToError, setCalibrationToError] = useItemFieldWithValidation<CalibratoinChangeRule, string>(rule, x => x.calibrationTo.toString(), validateCalibration);
	const [changeTime, setChangeTime] = useItemField<CalibratoinChangeRule, Dayjs | null>(rule, x => convertFromTimeSpan(x.changeTime));
	const [changeConsumption, setChangeConsumption, changeConsumptionError,] = useItemFieldWithValidation<CalibratoinChangeRule, string>(rule, x => x.changeConsumption.toString(), validateWidthChangeConsumption);

	useEffect(() => {
		const numberCalibrationTo = convertToNumber(calibrationTo); 
		
		if (numberCalibrationTo) {
			const sameRules = rules.filter((x, i) => i !== index && x.calibrationTo === numberCalibrationTo);

			if (sameRules.length > 0)
				setCalibrationToError(`Перенастройка на такое значение калибровки уже существует`);

			rule.calibrationTo = numberCalibrationTo;
			onValueChanged();
		}
	}, [calibrationTo, index, rules]);

	useEffect(() => {
		if (changeTime)
			rule.changeTime = convertToTimeSpan(changeTime);
	}, [changeTime]);

	useEffect(() => {
		const numberChangeConsumption = convertToNumber(changeConsumption); 
		
		if (numberChangeConsumption)
			rule.changeConsumption = numberChangeConsumption;
	}, [changeConsumption]);

	return (
		<RuleContainer>
			<BigFieldContainer>
				<InputField
					label='Калибровка, мм'
					value={calibrationTo}
					onChange={setCalibrationTo}
					errorText={calibrationToError}/>
			</BigFieldContainer>
			<FieldContainer>
				<TimeWithoutSelectField
					label='Время перенастройки'
					value={changeTime}
					onChange={setChangeTime}
					fullWidth/>
			</FieldContainer>
			<FieldContainer>
				<InputField
					label='Расход при перенастройке, кг/ч'
					value={changeConsumption}
					onChange={setChangeConsumption}
					errorText={changeConsumptionError}/>
			</FieldContainer>
			<DeleteButtonContainer>
				<DeleteButton onClick={() => onDelete(index)}/>
			</DeleteButtonContainer>
		</RuleContainer>
	);
}
import { Dayjs } from "dayjs";
import { useEffect } from "react";
import { InputField } from "../../common/inputs";
import { TimeWithoutSelectField } from "../../common/inputs/TimeField";
import { useItemFieldWithValidation, useItemField } from "../../common/useItemField";
import { validateCalibration } from "../../film-recipes/validations";
import { convertToNumber } from "../../utils/number-converters/numberConverter";
import { NozzleChangeRule } from "../productionLinesClient";
import { convertFromTimeSpan, convertToTimeSpan } from "../timespanConverter";
import { validateWidthChangeConsumption } from "../validations";
import { DeleteButton } from "../../common/elementControls";
import { RuleContainer, BigFieldContainer, FieldContainer, DeleteButtonContainer } from "./RuleComponents";

type NozzleChangeRuleComponentProps = {
    rule: NozzleChangeRule,
    index: number,
    onDelete: (index: number) => void,
    rules: NozzleChangeRule[],
	onValueChanged: () => void,
}

export const NozzleChangeRuleComponent = ({ rule, index, onDelete, rules, onValueChanged }: NozzleChangeRuleComponentProps) => {
	const [nozzleTo, setNozzleTo, nozzleToError, setNozzleToError] = useItemFieldWithValidation<NozzleChangeRule, string>(rule, x => x.nozzleTo.toString(), validateCalibration);
	const [changeTime, setChangeTime] = useItemField<NozzleChangeRule, Dayjs | null>(rule, x => convertFromTimeSpan(x.changeTime));
	const [changeConsumption, setChangeConsumption, changeConsumptionError, setchangeConsumptionError] = useItemFieldWithValidation<NozzleChangeRule, string>(rule, x => x.changeConsumption.toString(), validateWidthChangeConsumption);

	useEffect(() => {
		const numberNozzleTo = convertToNumber(nozzleTo); 
        
		if (numberNozzleTo) {
			const sameRules = rules.filter((x, i) => i !== index && x.nozzleTo === numberNozzleTo);

			if (sameRules.length > 0)
				setNozzleToError(`Перенастройка на такой диаметр сопла уже существует`);
            
			rule.nozzleTo = numberNozzleTo;
			onValueChanged();
		}
	}, [nozzleTo, index]);

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
					label='Диаметр сопла'
					value={nozzleTo}
					onChange={setNozzleTo}
					errorText={nozzleToError}/>
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
					label='Расход при перенастройке'
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
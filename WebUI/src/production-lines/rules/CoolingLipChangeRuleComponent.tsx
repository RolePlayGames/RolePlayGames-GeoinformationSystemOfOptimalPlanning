import { Dayjs } from "dayjs";
import { useEffect } from "react";
import { InputField } from "../../common/inputs";
import { TimeWithoutSelectField } from "../../common/inputs/TimeField";
import { useItemFieldWithValidation, useItemField } from "../../common/useItemField";
import { validateCalibration } from "../../film-recipes/validations";
import { convertToNumber } from "../../utils/number-converters/numberConverter";
import { CoolingLipChangeRule } from "../productionLinesClient";
import { convertFromTimeSpan, convertToTimeSpan } from "../timespanConverter";
import { validateWidthChangeConsumption } from "../validations";
import { DeleteButton } from "../../common/elementControls";
import { RuleContainer, BigFieldContainer, FieldContainer, DeleteButtonContainer } from "./RuleComponents";

type CoolingLipChangeRuleComponentProps = {
	rule: CoolingLipChangeRule,
	index: number,
	onDelete: (index: number) => void,
	rules: CoolingLipChangeRule[],
	onValueChanged: () => void,
}

export const CoolingLipChangeRuleComponent = ({ rule, index, onDelete, rules, onValueChanged }: CoolingLipChangeRuleComponentProps) => {
	const [coolingLipTo, setCoolingLipTo, coolingLipToError, setCoolingLipToError] = useItemFieldWithValidation<CoolingLipChangeRule, string>(rule, x => x.coolingLipTo.toString(), validateCalibration);
	const [changeTime, setChangeTime] = useItemField<CoolingLipChangeRule, Dayjs | null>(rule, x => convertFromTimeSpan(x.changeTime));
	const [changeConsumption, setChangeConsumption, changeConsumptionError, setchangeConsumptionError] = useItemFieldWithValidation<CoolingLipChangeRule, string>(rule, x => x.changeConsumption.toString(), validateWidthChangeConsumption);

	useEffect(() => {
		const numberCoolingLipTo = convertToNumber(coolingLipTo); 
		
		if (numberCoolingLipTo) {
			const sameRules = rules.filter((x, i) => i !== index && x.coolingLipTo === numberCoolingLipTo);

			if (sameRules.length > 0)
				setCoolingLipToError(`Перенастройка на такой зазор охлаждающих валиков уже существует`);

			rule.coolingLipTo = numberCoolingLipTo;
			onValueChanged();
		}
	}, [coolingLipTo, index]);

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
					label='Зазор охлаждающих роликов, мм'
					value={coolingLipTo}
					onChange={setCoolingLipTo}
					errorText={coolingLipToError}/>
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
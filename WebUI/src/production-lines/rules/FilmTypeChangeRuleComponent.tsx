import { Dayjs } from "dayjs";
import { useEffect } from "react";
import { SelectField, InputField } from "../../common/inputs";
import { TimeWithoutSelectField } from "../../common/inputs/TimeField";
import { useItemField, useItemFieldWithValidation } from "../../common/useItemField";
import { AvaliableFilmType } from "../../film-recipes/filmRecipesClient";
import { convertToNumber } from "../../utils/number-converters/numberConverter";
import { FilmTypeChangeRule } from "../productionLinesClient";
import { convertFromTimeSpan, convertToTimeSpan } from "../timespanConverter";
import { validateWidthChangeConsumption } from "../validations";
import { DeleteButton } from "../../common/elementControls";
import { RuleContainer, BigFieldContainer, FieldContainer, DeleteButtonContainer } from "./RuleComponents";

type FilmTypeChangeRuleComponentProps = {
	rule: FilmTypeChangeRule,
	index: number,
	onDelete: (index: number) => void,
	filmTypes: AvaliableFilmType[],
	rules: FilmTypeChangeRule[],
	onValueChanged: () => void,
}

export const FilmTypeChangeRuleComponent = ({ rule, index, onDelete, filmTypes, onValueChanged }: FilmTypeChangeRuleComponentProps) => {
	const [filmRecipeFromId, setFilmRecipeFromId] = useItemField<FilmTypeChangeRule, number | undefined>(rule, item => filmTypes.findIndex(x => x.id === item.filmRecipeFromID) > -1 ? item.filmRecipeFromID : filmTypes[0].id);
	const [filmRecipeToId, setFilmRecipeToId] = useItemField<FilmTypeChangeRule, number | undefined>(rule, item => filmTypes.findIndex(x => x.id === item.filmRecipeToID) > -1 ? item.filmRecipeToID : filmTypes[0].id);
	const [changeTime, setChangeTime] = useItemField<FilmTypeChangeRule, Dayjs | null>(rule, x => convertFromTimeSpan(x.changeTime));
	const [changeConsumption, setChangeConsumption, changeConsumptionError,] = useItemFieldWithValidation<FilmTypeChangeRule, string>(rule, x => x.changeConsumption.toString(), validateWidthChangeConsumption);

	useEffect(() => {
		if (filmRecipeFromId) {
			rule.filmRecipeFromID = filmRecipeFromId;
			onValueChanged();
		}
	}, [filmRecipeFromId]);

	useEffect(() => {
		if (filmRecipeToId) {
			rule.filmRecipeToID = filmRecipeToId;
			onValueChanged();
		}
	}, [filmRecipeToId]);

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
				<SelectField
					label='Начальный тип пленки'
					items={filmTypes}
					onItemChanged={setFilmRecipeFromId}
					variant='standard'
					value={filmRecipeFromId}/>
			</BigFieldContainer>
			<BigFieldContainer>
				<SelectField
					label='Конечный тип пленки'
					items={filmTypes}
					onItemChanged={setFilmRecipeToId}
					variant='standard'
					value={filmRecipeToId}/>
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
import { useNavigate } from "react-router-dom";
import { AddItemButton, HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { CalibratoinChangeRule, CoolingLipChangeRule, createProductionLine, deleteProductionLine, FilmTypeChangeRule, NozzleChangeRule, ProductionLine, updateProductionLine } from "./productionLinesClient";
import { convertToNumber } from "../utils/number-converters/numberConverter";
import { useItemField, useItemFieldWithValidation } from "../common/useItemField";
import { Dayjs } from "dayjs";
import { IClientError } from "../common/clients/clientError";
import { InputField } from "../common/inputs";
import { validateHourCost, validateMaxProductionSpeed, validateMaxThickness, validateMaxWidth, validateMinThickness, validateMinWidth, validateName, validateWidthChangeConsumption } from "./validations";
import { TimeWithoutSelectField } from "../common/inputs/TimeField";
import { AccordionDetails } from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import { CalibratoinChangeRuleComponent } from "./rules/CalibratoinChangeRuleComponent";
import { convertFromTimeSpan, convertToTimeSpan, defaultTimeSpan } from "./timespanConverter";
import { CoolingLipChangeRuleComponent } from "./rules/CoolingLipChangeRuleComponent";
import { FilmTypeChangeRuleComponent } from "./rules/FilmTypeChangeRuleComponent";
import { NozzleChangeRuleComponent } from "./rules/NozzleChangeRuleComponent";
import { AvaliableFilmType } from "../film-recipes/filmRecipesClient";
import { ChangeRuleList, ChangeRuleHeader } from "./rules/ChangeRuleList";
import { toast } from "react-toastify";

const calibrationChangeRulesCheck = (rules: CalibratoinChangeRule[]) => rules.filter(checkRule => rules.filter(rule => checkRule !== rule && checkRule.calibrationTo === rule.calibrationTo).length > 0).length == 0;
const coolingLipChangeRulesCheck = (rules: CoolingLipChangeRule[]) => rules.filter(checkRule => rules.filter(rule => checkRule !== rule && checkRule.coolingLipTo === rule.coolingLipTo).length > 0).length == 0;
const filmTypeChangeRulesCheck = (rules: FilmTypeChangeRule[]) => rules.filter(checkRule => rules.filter(rule => checkRule !== rule && checkRule.filmRecipeFromID === rule.filmRecipeFromID && checkRule.filmRecipeToID == rule.filmRecipeToID).length > 0).length == 0;
const nozzleChangeRulesCheck = (rules: NozzleChangeRule[]) => rules.filter(checkRule => rules.filter(rule => checkRule !== rule && checkRule.nozzleTo === rule.nozzleTo).length > 0).length == 0;

type ProductionLineElementProps = {
    id: number,
    item: ProductionLine,
    apiPath: string,
	filmTypes: AvaliableFilmType[],
}

export const ProductionLineElement = ({ id, item, apiPath, filmTypes }: ProductionLineElementProps) => {
	const [name, setName, nameError, setNameError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.name, validateName);
	const [hourCost, setHourCost, hourCostError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.hourCost.toString(), validateHourCost);
	const [maxProductionSpeed, setMaxProductionSpeed, maxProductionSpeedError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.maxProductionSpeed.toString(), validateMaxProductionSpeed);
	const [minWidth, setMinWidth, minWidthError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthMin.toString(), validateMinWidth);
	const [maxWidth, setMaxWidth, maxWidthError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthMax.toString(), x => validateMaxWidth(x, minWidth));
	const [minThickness, setMinThickness, minThicknessError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessMin.toString(), validateMinThickness);
	const [maxThickness, setMaxThickness, maxThicknessError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessMax.toString(), x => validateMaxThickness(x, minThickness));
	const [widthChangeTime, setWidthChangeTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.widthChangeTime));
	const [widthChangeConsumption, setWidthChangeConsumption, widthChangeConsumptionError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.widthChangeConsumption.toString(), validateWidthChangeConsumption);
	const [thicknessChangeTime, setThicknessChangeTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.thicknessChangeTime));
	const [thicknessChangeConsumption, setThicknessChangeConsumption, thicknessChangeConsumptionError] = useItemFieldWithValidation<ProductionLine, string>(item, x => x.thicknessChangeConsumption.toString(), validateWidthChangeConsumption);
	const [setupTime, setSetupTime] = useItemField<ProductionLine, Dayjs | null>(item, x => convertFromTimeSpan(x.setupTime));

	const [calibratoinChangeRules, setCalibratoinChangeRules] = useState(item.calibratoinChangeRules);
	const [coolingLipChangeRules, setCoolingLipChangeRules] = useState(item.coolingLipChangeRules);
	const [filmTypeChangeRules, setFilmTypeChangeRules] = useState(item.filmTypeChangeRules);
	const [nozzleChangeRules, setNozzleChangeRules] = useState(item.nozzleChangeRules);

	const [savingAvaliable, setSavingAvaliable] = useState(false);

	const navigate = useNavigate();

	const onUpdate = async (item: ProductionLine) => {
		try {
			await updateProductionLine(id, item);
			navigate(apiPath);
			toast.success(`Производственная линия ${name} была обновлена`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionLineNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при обновлении: указанное название уже используется в другой производственной линии`);
				setNameError('Указанное название уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);
			
		}
	};

	const onCreate = async (item: ProductionLine) => {
		try {
			await createProductionLine(item);
			navigate(apiPath);
			toast.success(`Производственная линия ${name} была обновлена`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionLineNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при создании: указанное название уже используется в другой производственной линии`);
				setNameError('Указанное название уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при создании. Проверьте параметры`);
			
		}
	};

	const onSave = useCallback(() => {
		const hourCostNumber = convertToNumber(hourCost);
		const maxProductionSpeedNumber = convertToNumber(maxProductionSpeed);
		const minWidthNumber = convertToNumber(minWidth);
		const maxWidthNumber = convertToNumber(maxWidth);
		const minThicknessNumber = convertToNumber(minThickness);
		const maxThicknessNumber = convertToNumber(maxThickness);
		const widthChangeConsumptionNumber = convertToNumber(widthChangeConsumption);
		const thicknessChangeConsumptionNumber = convertToNumber(thicknessChangeConsumption);

		if (hourCostNumber !== undefined
			&& maxProductionSpeedNumber !== undefined
			&& minWidthNumber !== undefined
			&& maxWidthNumber !== undefined
			&& minThicknessNumber !== undefined
			&& maxThicknessNumber !== undefined
            && widthChangeTime !== null
			&& widthChangeConsumptionNumber !== undefined
			&& thicknessChangeTime !== null
			&& thicknessChangeConsumptionNumber !== undefined
			&& setupTime !== null
			&& calibrationChangeRulesCheck(calibratoinChangeRules)
			&& coolingLipChangeRulesCheck(coolingLipChangeRules)
			&& filmTypeChangeRulesCheck(filmTypeChangeRules)
			&& nozzleChangeRulesCheck(nozzleChangeRules))
		{
			const item = {
				name: name,
				hourCost: hourCostNumber,
				maxProductionSpeed: maxProductionSpeedNumber,
				widthMin: minWidthNumber,
				widthMax: maxWidthNumber,
				thicknessMin: minThicknessNumber,
				thicknessMax: maxThicknessNumber,
				thicknessChangeTime: convertToTimeSpan(thicknessChangeTime),
				thicknessChangeConsumption: widthChangeConsumptionNumber,
				widthChangeTime: convertToTimeSpan(widthChangeTime),
				widthChangeConsumption: thicknessChangeConsumptionNumber,
				setupTime: convertToTimeSpan(setupTime),
				calibratoinChangeRules: calibratoinChangeRules,
				coolingLipChangeRules: coolingLipChangeRules,
				filmTypeChangeRules: filmTypeChangeRules,
				nozzleChangeRules: nozzleChangeRules,
			};

			if (id > 0) 
				return onUpdate(item);
			else 
				return onCreate(item);
		}
		
		return Promise.resolve();
	}, [hourCost,
		maxProductionSpeed,
		minWidth,
		maxWidth,
		minThickness,
		maxThickness,
		widthChangeConsumption,
		thicknessChangeConsumption,
		calibratoinChangeRules,
		coolingLipChangeRules,
		filmTypeChangeRules,
		nozzleChangeRules]);

	const onDelete = async () => {
		if (id > 0) {
			await deleteProductionLine(id);
			navigate(apiPath);
		}
	};

	const updateSavingAvaliable = useCallback(() => {
		const hourCostNumber = convertToNumber(hourCost);
		const maxProductionSpeedNumber = convertToNumber(maxProductionSpeed);
		const minWidthNumber = convertToNumber(minWidth);
		const maxWidthNumber = convertToNumber(maxWidth);
		const minThicknessNumber = convertToNumber(minThickness);
		const maxThicknessNumber = convertToNumber(maxThickness);
		const widthChangeConsumptionNumber = convertToNumber(widthChangeConsumption);
		const thicknessChangeConsumptionNumber = convertToNumber(thicknessChangeConsumption);

		console.log(`Check started: ${hourCostNumber !== undefined}
			&& ${maxProductionSpeedNumber !== undefined}
			&& ${minWidthNumber !== undefined}
			&& ${maxWidthNumber !== undefined}
			&& ${minThicknessNumber !== undefined}
			&& ${maxThicknessNumber !== undefined}
            && ${widthChangeTime !== null}
			&& ${widthChangeConsumptionNumber !== undefined}
			&& ${thicknessChangeTime !== null}
			&& ${thicknessChangeConsumptionNumber !== undefined}
			&& ${setupTime !== null}
			&& ${calibrationChangeRulesCheck(calibratoinChangeRules)}
			&& ${coolingLipChangeRulesCheck(coolingLipChangeRules)}
			&& ${filmTypeChangeRulesCheck(filmTypeChangeRules)}
			&& ${nozzleChangeRulesCheck(nozzleChangeRules)}`);

		setSavingAvaliable(hourCostNumber !== undefined
			&& maxProductionSpeedNumber !== undefined
			&& minWidthNumber !== undefined
			&& maxWidthNumber !== undefined
			&& minThicknessNumber !== undefined
			&& maxThicknessNumber !== undefined
            && widthChangeTime !== null
			&& widthChangeConsumptionNumber !== undefined
			&& thicknessChangeTime !== null
			&& thicknessChangeConsumptionNumber !== undefined
			&& setupTime !== null
			&& calibrationChangeRulesCheck(calibratoinChangeRules)
			&& coolingLipChangeRulesCheck(coolingLipChangeRules)
			&& filmTypeChangeRulesCheck(filmTypeChangeRules)
			&& nozzleChangeRulesCheck(nozzleChangeRules));
	}, [hourCost,
		maxProductionSpeed,
		minWidth,
		maxWidth,
		minThickness,
		maxThickness,
		widthChangeConsumption,
		thicknessChangeConsumption,
		calibratoinChangeRules,
		coolingLipChangeRules,
		filmTypeChangeRules,
		nozzleChangeRules]);

	useEffect(() => {
		updateSavingAvaliable();
	}, [hourCost,
		maxProductionSpeed,
		minWidth,
		maxWidth,
		minThickness,
		maxThickness,
		widthChangeConsumption,
		thicknessChangeConsumption,
		calibratoinChangeRules,
		coolingLipChangeRules,
		filmTypeChangeRules,
		nozzleChangeRules]);

	const onAddCalibrationChangeRule = () => {
		item.calibratoinChangeRules = [...item.calibratoinChangeRules, { calibrationTo: 0, changeTime: defaultTimeSpan, changeConsumption: 0 } ];
		setCalibratoinChangeRules(item.calibratoinChangeRules);
	};

	const onDeleteCalibrationChangeRule = (index: number) => {
		item.calibratoinChangeRules = item.calibratoinChangeRules.filter((_, i) => i !== index);
		setCalibratoinChangeRules(item.calibratoinChangeRules);
	};

	const onAddCoolingLipChangeRule = () => {
		item.coolingLipChangeRules = [...item.coolingLipChangeRules, { coolingLipTo: 0, changeTime: defaultTimeSpan, changeConsumption: 0 } ];
		setCoolingLipChangeRules(item.coolingLipChangeRules);
	};

	const onDeleteCoolingLipChangeRule = (index: number) => {
		item.coolingLipChangeRules = item.coolingLipChangeRules.filter((_, i) => i !== index);
		setCoolingLipChangeRules(item.coolingLipChangeRules);
	};

	const onAddFilmTypeChangeRule = () => {
		item.filmTypeChangeRules = [...item.filmTypeChangeRules, { filmRecipeFromID: 0, filmRecipeToID: 0, changeTime: defaultTimeSpan, changeConsumption: 0 } ];
		setFilmTypeChangeRules(item.filmTypeChangeRules);
	};

	const onDeleteFilmTypeChangeRule = (index: number) => {
		item.filmTypeChangeRules = item.filmTypeChangeRules.filter((_, i) => i !== index);
		setFilmTypeChangeRules(item.filmTypeChangeRules);
	};

	const onAddNozzleChangeRule = () => {
		item.nozzleChangeRules = [...item.nozzleChangeRules, { nozzleTo: 0, changeTime: defaultTimeSpan, changeConsumption: 0 } ];
		setNozzleChangeRules(item.nozzleChangeRules);
	};

	const onDeleteNozzleChangeRule = (index: number) => {
		item.nozzleChangeRules = item.nozzleChangeRules.filter((_, i) => i !== index);
		setNozzleChangeRules(item.nozzleChangeRules);
	};

	return(
		<ElementContainer>
			<HeaderLabel>Производственная линия {item.name}</HeaderLabel>
			<ActionsBar>
				<SaveButton
					onClick={onSave}
					disabled={!savingAvaliable}
				/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<InputField
				label='Название'
				value={name}
				onChange={setName}
				errorText={nameError}/>
			<InputField
				label='Стоимость часа работы, у.е./ч.'
				value={hourCost}
				onChange={setHourCost}
				errorText={hourCostError}/>
			<InputField
				label='Максимальная скорость производства, у.е./час'
				value={maxProductionSpeed}
				onChange={setMaxProductionSpeed}
				errorText={maxProductionSpeedError}/>
			<InputField
				label='Минимальная ширина рулона, мм'
				value={minWidth}
				onChange={setMinWidth}
				errorText={minWidthError}/>
			<InputField
				label='Максимальная ширина рулона, мм'
				value={maxWidth}
				onChange={setMaxWidth}
				errorText={maxWidthError}/>
			<InputField
				label='Минимальная толщина рулона, мкм'
				value={minThickness}
				onChange={setMinThickness}
				errorText={minThicknessError}/>
			<InputField
				label='Максимальная толщина рулона, мкм'
				value={maxThickness}
				onChange={setMaxThickness}
				errorText={maxThicknessError}/>
			<TimeWithoutSelectField
				label='Время перенастройки по ширине'
				value={widthChangeTime}
				onChange={setWidthChangeTime}/>
			<InputField
				label='Расход при перенастройке по ширине, кг/час'
				value={widthChangeConsumption}
				onChange={setWidthChangeConsumption}
				errorText={widthChangeConsumptionError}/>
			<TimeWithoutSelectField
				label='Время перенастройки по толщине'
				value={thicknessChangeTime}
				onChange={setThicknessChangeTime}/>
			<InputField
				label='Расход при перенастройке по толщине, кг/час'
				value={thicknessChangeConsumption}
				onChange={setThicknessChangeConsumption}
				errorText={thicknessChangeConsumptionError}/>
			<TimeWithoutSelectField
				label='Время прогрева'
				value={setupTime}
				onChange={setSetupTime}/>
			<ChangeRuleList defaultExpanded>
				<ChangeRuleHeader>Правила перенастройки по калибровке</ChangeRuleHeader>
				<AccordionDetails>
					<AddItemButton onClick={onAddCalibrationChangeRule}/>
					{ item.calibratoinChangeRules.map((rule, index) => (
						<CalibratoinChangeRuleComponent
							rule={rule}
							index={index}
							onDelete={onDeleteCalibrationChangeRule}
							rules={calibratoinChangeRules}
							onValueChanged={updateSavingAvaliable}/>
					)) }
				</AccordionDetails>
			</ChangeRuleList>
			<ChangeRuleList defaultExpanded>
				<ChangeRuleHeader>Правила перенастройки по охлождающим роликам</ChangeRuleHeader>
				<AccordionDetails>
					<AddItemButton onClick={onAddCoolingLipChangeRule}/>
					{ item.coolingLipChangeRules.map((rule, index) => (
						<CoolingLipChangeRuleComponent
							rule={rule}
							index={index}
							onDelete={onDeleteCoolingLipChangeRule}
							rules={coolingLipChangeRules}
							onValueChanged={updateSavingAvaliable}/>
					)) }
				</AccordionDetails>
			</ChangeRuleList>
			<ChangeRuleList defaultExpanded>
				<ChangeRuleHeader>Правила перенастройки по типу пленки</ChangeRuleHeader>
				<AccordionDetails>
					<AddItemButton onClick={onAddFilmTypeChangeRule}/>
					{ item.filmTypeChangeRules.map((rule, index) => (
						<FilmTypeChangeRuleComponent
							rule={rule}
							index={index}
							onDelete={onDeleteFilmTypeChangeRule}
							filmTypes={filmTypes}
							rules={filmTypeChangeRules}
							onValueChanged={updateSavingAvaliable}/>
					)) }
				</AccordionDetails>
			</ChangeRuleList>
			<ChangeRuleList defaultExpanded>
				<ChangeRuleHeader>Правила перенастройки по диаметру сопла</ChangeRuleHeader>
				<AccordionDetails>
					<AddItemButton onClick={onAddNozzleChangeRule}/>
					{ item.nozzleChangeRules.map((rule, index) => (
						<NozzleChangeRuleComponent
							rule={rule}
							index={index}
							onDelete={onDeleteNozzleChangeRule}
							rules={nozzleChangeRules}
							onValueChanged={updateSavingAvaliable}/>
					)) }
				</AccordionDetails>
			</ChangeRuleList>
		</ElementContainer>
	);
}
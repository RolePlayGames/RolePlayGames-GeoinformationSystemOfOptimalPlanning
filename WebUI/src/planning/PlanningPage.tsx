import styled from "@emotion/styled";
import { Box, Button, Checkbox, Divider, FormControl, FormControlLabel, FormLabel,  List, ListItem, ListItemButton, ListItemIcon, ListItemText, Radio, RadioGroup, Step, StepContent, stepIconClasses, StepLabel, Stepper, Typography } from "@mui/material";
import { HeaderLabel, PageContainer } from "../common/controls";
import { useCallback, useEffect, useState } from "react";
import { ListItem as Item } from "../common/Item";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { DateRangeCalendar } from '@mui/x-date-pickers-pro/DateRangeCalendar';
import { LoadingProgress } from "../common/LoadingProgress";
import { getOrdersInfo } from "../orders/ordersClient";
import { getProductionLinesInfo } from "../production-lines/productionLinesClient";
import { DateRange } from "@mui/x-date-pickers-pro";
import dayjs, { Dayjs } from "dayjs";
import { ActionsBar } from "../common/elementControls";
import { TimeWithoutSelectField } from "../common/inputs/TimeField";
import { FieldContainer } from "../production-lines/rules/RuleComponents";
import { useFieldWithValidation } from "../common/useItemField";
import { convertToInt, convertToNumber } from "../utils/number-converters/numberConverter";
import { InputField } from "../common/inputs";
import { planningByBruteforce, planningByGenetic, ProductionPlanInfo } from "./planningClient";
import { convertToTimeSpan } from "../production-lines/timespanConverter";
import { validateMaxIterationsCount, validateGenerationsCount, validateMutationCoefficient, validateMutationSelectionCount, validateCrossoverSelectionCount, validateIndividualsInPopulationCount, validateCrossoverPointsCount, validatePointedMutationProbability, validateStartPopulationsCount } from "./validations";
import { oldPlanOrderNumbers, oldPlanProductionLinesNames } from "./oldPlanProductionData";
import { steps } from "./steps";
import { Gantt, Task, ViewMode } from "gantt-task-react";
import { convertProductionPlanToTasks } from "./productionPlanConverter";
import "gantt-task-react/dist/index.css";
import 'dayjs/locale/ru'; // Import your desired locale (Russian in this case)

const PlanningStepContainer = styled(FormControl)({
	width: 'fill-available',
});

const PlanningStep = styled(Step)({
	'& .MuiStepLabel-root .Mui-completed': {
		color: 'black',
	},
	'& .MuiStepLabel-label.Mui-completed.MuiStepLabel-alternativeLabel':
	{
		color: 'white',
	},
	'& .MuiStepLabel-root .Mui-active': {
		color: 'black',
	},
	'& .MuiStepLabel-label.Mui-active.MuiStepLabel-alternativeLabel':
	{
		color: 'white',
	},
	'& .MuiStepLabel-root .Mui-active .MuiStepIcon-text': {
		fill: 'white',
	},
});

const BlackLabel = styled(FormLabel)({
	color: 'black !important',
	fontSize: '1.3rem'
});

const formatMinutesToDDHHMMSS = (totalMinutes: number) => {
	const seconds = Math.floor((totalMinutes * 60) % 60);
	const hours = Math.floor((totalMinutes / 60) % 24);
	const days = Math.floor(totalMinutes / (60 * 24));
	const minutes = Math.floor(totalMinutes % 60);

	const formattedDays = days > 0 ? `${days} дней ` : '';
	const formattedHours = hours > 0 || days > 0 ? `${hours.toString().padStart(2, '0')} часов ` : ''; //Show hours if days
	const formattedMinutes = minutes > 0 || hours > 0 || days > 0 ? `${minutes.toString().padStart(2, '0')} минут ` : ''; //Show minutes if days or hours
	const formattedSeconds = seconds > 0 || minutes > 0 || hours > 0 || days > 0 ? `${seconds.toString().padStart(2, '0')} секунд` : '';

	return `${formattedDays}${formattedHours}${formattedMinutes}${formattedSeconds}`.trim();
}

const removeDivWithLabel = (parentElement: HTMLElement, labelText: string) => {
	if(parentElement){
		const divs = parentElement.querySelectorAll("div");

		divs.forEach(div => {
			const label = div.querySelector("label");
			if (label && label.textContent === labelText) {
				div.remove();
				console.log("Div removed successfully.");
			}
		});
	} else 
		console.log("Parent element not found");
	
}

type AlgorithmType = 'Bruteforce' | 'Genetic';

const PlanningContianer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
});

export const PlanningPage = () => {
	const [activeStep, setActiveStep] = useState(0)
	const [isNextStepDisabled, setIsNextStepDisabled] = useState(false)

	const [functionType, setFunctionType] = useState(0)
	
	const [planningInterval, setPlanningInterval] = useState<DateRange<Dayjs>>([
		dayjs(),
		dayjs(),
	]);

	const [orders, setOrders] = useState<Item[]>()
	const [selectedOrders, setSelectedOrders] = useState<number[]>([])

	const [productionLines, setProductionLines] = useState<Item[]>()
	const [selectedProductionLines, setSelectedProductionLines] = useState<number[]>([])

	const [algorithmType, setAlgorithmType] = useState<AlgorithmType>('Bruteforce')
	const [timeoutDelay, setTimeoutDelay, timeoutDelayError, setTimeoutDelayError] = useFieldWithValidation<Dayjs | null>(null, () => '')
	const [iterationsCount, setIterationsCount, iterationsCountError, setIterationsCountError] = useFieldWithValidation<string>('', validateMaxIterationsCount)
	const [generationsCount, setGeneratinosCount, generationsCountError] = useFieldWithValidation<string>('1000', validateGenerationsCount)
	const [mutationCoefficient, setMutationCoefficient, mutationCoefficientError] = useFieldWithValidation<string>('0.06', validateMutationCoefficient)
	const [mutationSelectionCount, setMutationSelectionCount, mutationSelectionCountError] = useFieldWithValidation<string>('25', validateMutationSelectionCount)
	const [crossoverSelectionCount, setCrossoverSelectionCount, crossoverSelectionCountError] = useFieldWithValidation<string>('100', validateCrossoverSelectionCount)
	const [individualsInPopulationCount, setIndividualsInPopulationCount, individualsInPopulationCountError] = useFieldWithValidation<string>('3', validateIndividualsInPopulationCount)
	const [crossoverPointsCount, setCrossoverPointsCount, crossoverPointsCountError] = useFieldWithValidation<string>('30', validateCrossoverPointsCount)
	const [pointedMutationProbability, setPointedMutationProbability, pointedMutationProbabilityError] = useFieldWithValidation<string>('0,15', validatePointedMutationProbability)
	const [startPopulationsCount, setStartPopulationsCount, startPopulationsCountError] = useFieldWithValidation<string>('200', validateStartPopulationsCount)

	const [planInfos, setPlanInfos] = useState<ProductionPlanInfo[]>();
	const [tasks, setTasks] = useState<Task[]>();
	const [targetFunctionResult, setTargetFunctionResult] = useState<string>('');
	const [viewMode, setViewMode] = useState<ViewMode>(ViewMode.Day);

	const handleNext = async () => {
		setActiveStep((prevActiveStep) => prevActiveStep + 1);
	}
  
	const handleBack = () => {
		setActiveStep((prevActiveStep) => prevActiveStep - 1);
	};
  
	const handleReset = () => {
		setActiveStep(0);
	};

	useEffect(() => {
		if (activeStep === 2 && selectedOrders.length === 0) 
			setIsNextStepDisabled(true);
		else if (activeStep === 3 && selectedProductionLines.length === 0)
			setIsNextStepDisabled(true);
		else if (activeStep === 4 && algorithmType === 'Bruteforce')
			if (iterationsCountError)
				setIsNextStepDisabled(true);
			else if (timeoutDelay === null && iterationsCount === '') {
				setIsNextStepDisabled(true);
				setTimeoutDelayError('Заполните хотя бы одно из этих полей');
				setIterationsCountError('Заполните хотя бы одно из этих полей');
			} else {
				setTimeoutDelayError('');
				setIterationsCountError('');
				setIsNextStepDisabled(false);
			}
		else if (activeStep === 4 && algorithmType === 'Genetic') {
			if (generationsCountError
					|| mutationCoefficientError
					|| mutationSelectionCountError
					|| crossoverSelectionCountError
					|| individualsInPopulationCountError
					|| crossoverPointsCountError
					|| pointedMutationProbabilityError
					|| startPopulationsCountError) 
				setIsNextStepDisabled(true);

			setTimeoutDelayError('');
			setIterationsCountError('');
			setIsNextStepDisabled(false);
		} else 
			setIsNextStepDisabled(false);
		
	}, [
		activeStep,
		selectedOrders,
		selectedProductionLines,
		algorithmType,
		timeoutDelay,
		iterationsCount,
		generationsCountError,
		iterationsCountError,
		mutationCoefficientError,
		mutationSelectionCountError,
		crossoverSelectionCountError,
		individualsInPopulationCountError,
		crossoverPointsCountError,
		pointedMutationProbabilityError,
		startPopulationsCountError
	]);
  
	const handleFunctionTypeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setFunctionType(Number((event.target as HTMLInputElement).value));
	};
	
	const loadOrders = useCallback(async () => {
		const items = await getOrdersInfo();      
		setOrders(items);
	}, []);

	const selectOldPlanOrders = () => {
		if (orders !== undefined) {
			const newSelectedOrders = orders/*.filter(order => oldPlanOrderNumbers.includes(order.name))*/.map(order => order.id);
			setSelectedOrders(newSelectedOrders);
		}
	};

	const selectOldPlanProductionLines = () => {
		if (productionLines !== undefined) {
			const newSelectedProductionLines = productionLines.filter(line => oldPlanProductionLinesNames.includes(line.name)).map(order => order.id);
			setSelectedProductionLines(newSelectedProductionLines);
		}
	};

	const handleOrderSelected = (value: number) => () => {
		const currentIndex = selectedOrders.indexOf(value);
		const newChecked = [...selectedOrders];

		if (currentIndex === -1) 
			newChecked.push(value);
		else 
			newChecked.splice(currentIndex, 1);

		setSelectedOrders(newChecked);
	};
	
	const loadProductionLines = useCallback(async () => {
		const items = await getProductionLinesInfo();      
		setProductionLines(items);
	}, []);

	const handleProductionLinesSelected = (value: number) => () => {
		const currentIndex = selectedProductionLines.indexOf(value);
		const newChecked = [...selectedProductionLines];

		if (currentIndex === -1) 
			newChecked.push(value);
		else 
			newChecked.splice(currentIndex, 1);

		setSelectedProductionLines(newChecked);
	};
  
	const handleAlgorithmTypeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setAlgorithmType((event.target as HTMLInputElement).value as AlgorithmType);
	};

	useEffect(() => {
		if (activeStep === 2 && orders === undefined) 
			loadOrders();
		else if (activeStep === 3 && productionLines === undefined) 
			loadProductionLines();
		
	}, [activeStep]);

	useEffect(() => {
		if (activeStep === steps.length) {
			const tryRunPlanning = async () => {
				const startDateTime = planningInterval[0]?.toDate();
				console.log(`loadOptimalPlan: algorithmType: ${algorithmType}`);
	
				if (algorithmType === 'Bruteforce') {
					const iterationsCountNum = convertToInt(iterationsCount);
					console.log(`startDateTime: ${startDateTime}; iterationsCount: ${iterationsCount}; iterationsCountNum: ${iterationsCountNum}; selectedOrders: ${selectedOrders}`);
	
					if (startDateTime && (iterationsCountNum || timeoutDelay !== null)) 
						try {
							const plans = await planningByBruteforce({
								startDateTime: startDateTime,
								orders: selectedOrders,
								productionLines: selectedProductionLines,
								functionType: functionType,
								conditions: {
									timeoutDelay: timeoutDelay === null ? undefined : convertToTimeSpan(timeoutDelay),
									iterationsCount: iterationsCountNum,
								}
							});
	
							setPlanInfos(plans.sort((a, b) => b.targetFunctionValue - a.targetFunctionValue));
						} catch (error: unknown) {
							console.log(`Catch an error while trying planning by bruteforce ${error}`);
							setActiveStep((prevActiveStep) => prevActiveStep - 1);
						}
					else
						setActiveStep((prevActiveStep) => prevActiveStep - 1);
					
				} else {
					const generationsCountNum = convertToInt(generationsCount);
					const mutationCoefficientNum = convertToNumber(mutationCoefficient);
					const mutationSelectionCountNum = convertToInt(mutationSelectionCount);
					const crossoverSelectionCountNum = convertToInt(crossoverSelectionCount);
					const individualsInPopulationCountNum = convertToInt(individualsInPopulationCount);
					const crossoverPointsCountNum = convertToInt(crossoverPointsCount);
					const pointedMutationProbabilityNum = convertToNumber(pointedMutationProbability);
					const startPopulationsCountNum = convertToInt(startPopulationsCount);
	
					if (startDateTime
						&& generationsCountNum
						&& mutationCoefficientNum
						&& mutationSelectionCountNum
						&& crossoverSelectionCountNum
						&& individualsInPopulationCountNum
						&& crossoverPointsCountNum
						&& pointedMutationProbabilityNum
						&& startPopulationsCountNum) 
						try {
							const plans = await planningByGenetic({
								startDateTime: startDateTime,
								orders: selectedOrders,
								productionLines: selectedProductionLines,
								functionType: functionType,
								conditions: {
									timeoutDelay: timeoutDelay === null ? undefined : convertToTimeSpan(timeoutDelay),
									iterationsCount: undefined,
									generationsCount: generationsCountNum,
								},
								options : {
									mutationCoefficient: mutationCoefficientNum,
									mutationSelectionCount: mutationSelectionCountNum,
									crossoverSelectionCount: crossoverSelectionCountNum,
									individualsInPopulationCount: individualsInPopulationCountNum,
									crossoverPointsCount: crossoverPointsCountNum,
									pointedMutationProbability: pointedMutationProbabilityNum,
									startPopulationsCount: startPopulationsCountNum,
								}
							});
	
							setPlanInfos(plans.sort((a, b) => b.targetFunctionValue - a.targetFunctionValue));
						} catch (error: unknown) {
							console.log(`Catch an error while trying planning by genetic ${error}`);
							setActiveStep((prevActiveStep) => prevActiveStep - 1);
						}
					else
						setActiveStep((prevActiveStep) => prevActiveStep - 1);
				}
			}
	
			tryRunPlanning();
		}
	}, [
		activeStep,
		planningInterval,
		algorithmType,
		selectedOrders,
		selectedProductionLines,
		functionType,
		timeoutDelay,
		generationsCount,
		mutationCoefficient,
		mutationSelectionCount,
		crossoverSelectionCount,
		individualsInPopulationCount,
		crossoverPointsCount,
		pointedMutationProbability,
		startPopulationsCount,
	]);

	useEffect(() => {
		if (planInfos && planInfos.length > 0) {
			console.log('Start converting production plan')
			const newTasks = convertProductionPlanToTasks(planInfos[planInfos.length - 1]);
			setTasks(newTasks);
			setTargetFunctionResult(formatMinutesToDDHHMMSS(planInfos[planInfos.length - 1].targetFunctionValue));
			console.log('End converting production plan')
		}
	}, [planInfos]);

	useEffect(() => {
		if (activeStep === 1) {
			const parentElement = document.getElementById('MyDateRangeCalendarPicker');
			const labelText = "MUI X Missing license key";

			if (parentElement) 
				removeDivWithLabel(parentElement, labelText);
		}
	}, [activeStep]);

	const handleViewAllPlans = () => {
		setTasks(undefined);
	};

	const handleReturnToPlanning = () => {
		setTasks(undefined);
		setPlanInfos(undefined);
		setActiveStep(0);
	};

	const handlePlanSelected = (value: number) => () => {
		if (planInfos) {
			const newTasks = convertProductionPlanToTasks(planInfos[value]);
			setTasks(newTasks);
			setTargetFunctionResult(formatMinutesToDDHHMMSS(planInfos[value].targetFunctionValue));
		}
	};

	return (
		<PageContainer sx={{ height: '100vh' }}>
			<HeaderLabel>Оптимальное планирование</HeaderLabel>
			<PlanningContianer sx={{ overflow: 'auto' }}>
				{ activeStep === steps.length ? (
					<>
						{ planInfos ? (
							<>
								{ tasks ? (
									<Box sx={{ display: 'flex', flexDirection: 'column' }}>
										<Box sx={{
											display: 'flex',
											flexDirection: 'row',
											marginBottom: '1vw',
											marginTop: '15px',
											justifyContent: 'flex-start',
										}}>
											<Button
												variant="contained"
												sx={{
													background: '#1d1b31',
													'&:hover': {
														backgroundColor: '#11101d'
													},
													margingLeft: '20px',
												}}
												onClick={handleViewAllPlans}
											>
												Остальные решения
											</Button>
											<Button
												variant="contained"
												sx={{
													background: '#1d1b31',
													'&:hover': {
														backgroundColor: '#11101d'
													},
													margingLeft: '20px',
												}}
												onClick={handleReturnToPlanning}
											>
												Вернуться к планированию
											</Button>
											<Typography sx={{ color: 'black !important', fontSize: '1.3rem', margingLeft: '25px', }}>Значение критерия: {targetFunctionResult}</Typography>
										</Box>
										<Gantt tasks={tasks} viewMode={viewMode}/>
									</Box>
								) : (
									<Box sx={{ margin: '20px', width: '-webkit-fill-available' }}>
										<PlanningStepContainer>
											<BlackLabel>Поэтапный список решений</BlackLabel>
											<List sx={{ width: '100%' }}>
												{ planInfos.map((value, index) => (
													<ListItem key={index} disablePadding>
														<ListItemButton role={undefined} onClick={handlePlanSelected(index)}>
															<ListItemText primary={`${index + 1} Производственный план (${formatMinutesToDDHHMMSS(value.targetFunctionValue)})`} />
														</ListItemButton>
													</ListItem>
												))}
											</List>
										</PlanningStepContainer>
									</Box>
								)}
							</>
						) : (
							<LoadingProgress/>
						)}
					</>
				) : (
					<>
						<Box sx={{ maxWidth: 400, margin: '20px' }}>
							<Stepper activeStep={activeStep} orientation="vertical">
								{steps.map((step, index) => (
									<PlanningStep key={step.label}>
										<StepLabel
											optional={
												index === steps.length - 1 ? (
													<Typography variant="caption">Последний шаг</Typography>
												) : null
											}>
											{step.label}
										</StepLabel>
										<StepContent>
											<Typography>{step.description}</Typography>
											<Box sx={{ mb: 2 }}>
												<Button
													variant="contained"
													onClick={handleNext}
													sx={{ mt: 1, mr: 1, background: '#1d1b31', '&:hover': { backgroundColor: '#11101d' }}}
													disabled={isNextStepDisabled}
												>
													{index === steps.length - 1 ? 'Начать планирование' : 'Далее'}
												</Button>
												<Button
													disabled={index === 0}
													onClick={handleBack}
													sx={{ mt: 1, mr: 1, color: 'black'}}
												>
													Назад
												</Button>
											</Box>
										</StepContent>
									</PlanningStep>
								))}
							</Stepper>
						</Box>
						<Divider orientation="vertical" flexItem />
						<Box sx={{ margin: '20px', width: '-webkit-fill-available' }}>
							{activeStep === 0 && (
								<PlanningStepContainer>
									<BlackLabel>Выберите критерий оптимального планирования</BlackLabel>
									<RadioGroup value={functionType} onChange={handleFunctionTypeChange}>
										<FormControlLabel value={0} control={<Radio />} label="Время производства" />
										<FormControlLabel value={1} control={<Radio />} label="Стоимость производства" />
									</RadioGroup>
								</PlanningStepContainer>
							)}
							{activeStep === 1 && (
								<PlanningStepContainer id='MyDateRangeCalendarPicker'>
									<FormLabel sx={{ color: 'black !important', fontWeight: 'bold', fontSize: '1.3rem' }}>Выберите период планирования</FormLabel>
									<LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="ru">
										<DemoContainer components={['DateRangeCalendar']}>
											<DateRangeCalendar value={planningInterval} onChange={(newValue) => setPlanningInterval(newValue)}/>
										</DemoContainer>
									</LocalizationProvider>
								</PlanningStepContainer>
							)}
							{activeStep === 2 && (
								<PlanningStepContainer>
									<BlackLabel>Выберите заказы, участвующие в оптимальном планировании</BlackLabel>
									<ActionsBar sx={{ marginTop: '15px' }}>
										<Button
											variant="contained"
											onClick={() => setSelectedOrders([])}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Очистить выбранное
										</Button>
										<Button
											variant="contained"
											onClick={() => setSelectedOrders((selected) => orders ? orders.map(x => x.id) : selected)}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Выбрать все
										</Button>
										<Button
											variant="contained"
											onClick={selectOldPlanOrders}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Выбрать заказы из оригинального плана
										</Button>
									</ActionsBar>
									{ orders === undefined ? (
										<LoadingProgress/>
									) : (
										<List sx={{ width: '100%' }}>
											{orders.map((value) => {
												const labelId = `checkbox-list-label-${value}`;
												return (
													<ListItem key={value.id} disablePadding>
														<ListItemButton role={undefined} onClick={handleOrderSelected(value.id)} dense>
															<ListItemIcon>
																<Checkbox
																	edge="start"
																	checked={selectedOrders.includes(value.id)}
																	tabIndex={-1}
																	disableRipple
																	inputProps={{ 'aria-labelledby': labelId }}
																/>
															</ListItemIcon>
															<ListItemText id={labelId} primary={`${value.name}`} />
														</ListItemButton>
													</ListItem>
												);
											})}
										</List>
									)}
								</PlanningStepContainer>
							)}
							{activeStep === 3 && (
								<PlanningStepContainer>
									<BlackLabel>Выберите производственные линии, участвующие в оптимальном планировании</BlackLabel>
									<ActionsBar sx={{ marginTop: '15px' }}>
										<Button
											variant="contained"
											onClick={() => setSelectedProductionLines([])}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Очистить выбранное
										</Button>
										<Button
											variant="contained"
											onClick={() => setSelectedProductionLines((selected) => productionLines ? productionLines.map(x => x.id) : selected)}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Выбрать все
										</Button>
										<Button
											variant="contained"
											onClick={selectOldPlanProductionLines}
											sx={{
												background: '#1d1b31',
												'&:hover': {
													backgroundColor: '#11101d'
												}
											}}
										>
											Выбрать линии из оригинального плана
										</Button>
									</ActionsBar>
									{ productionLines === undefined ? (
										<LoadingProgress/>
									) : (
										<List sx={{ width: '100%' }}>
											{ productionLines.map((value) => {
												const labelId = `checkbox-list-label-${value}`;
												return (
													<ListItem key={value.id} disablePadding>
														<ListItemButton role={undefined} onClick={handleProductionLinesSelected(value.id)} dense>
															<ListItemIcon>
																<Checkbox
																	edge="start"
																	checked={selectedProductionLines.includes(value.id)}
																	tabIndex={-1}
																	disableRipple
																	inputProps={{ 'aria-labelledby': labelId }}
																/>
															</ListItemIcon>
															<ListItemText id={labelId} primary={`${value.name}`} />
														</ListItemButton>
													</ListItem>
												);
											})}
										</List>
									)}
								</PlanningStepContainer>
							)}
							{activeStep === 4 && (
								<PlanningStepContainer>
									<BlackLabel>Выберите критерий оптимального планирования</BlackLabel>
									<RadioGroup value={algorithmType} onChange={handleAlgorithmTypeChange}>
										<FormControlLabel value={'Bruteforce'} control={<Radio />} label="Алгоритм полного перебора" />
										<FormControlLabel value={'Genetic'} control={<Radio />} label="Генетический алгоритм" />
									</RadioGroup>
									<PlanningStepContainer>
										<FormLabel sx={{ color: 'black !important', marginTop: '10px', fontSize: 'large' }}>Настройки алгоритма</FormLabel>
										{ algorithmType === 'Bruteforce' ? (
											<Box sx={{
												display: 'flex',
												flexDirection: 'column',
												width: 'fill-available',
												marginLeft: '2vw',
												marginRight: '2vw',
											}}>
												<FieldContainer>
													<TimeWithoutSelectField
														label='Максимальное время работы алгоритма'
														value={timeoutDelay}
														onChange={setTimeoutDelay}
														error={!!timeoutDelayError}
														helperText={timeoutDelayError}
														fullWidth/>
												</FieldContainer>
												<InputField
													label='Максимальное количество итераций'
													value={iterationsCount}
													onChange={setIterationsCount}
													errorText={iterationsCountError}/>
											</Box>
										) : (
											<Box sx={{
												display: 'flex',
												flexDirection: 'column',
												width: 'fill-available',
												marginLeft: '2vw',
												marginRight: '2vw',
											}}>
												<InputField
													label='Количество поколений'
													value={generationsCount}
													onChange={setGeneratinosCount}
													errorText={generationsCountError}/>
												<FieldContainer>
													<TimeWithoutSelectField
														label='Максимальное время работы алгоритма'
														value={timeoutDelay}
														onChange={setTimeoutDelay}
														error={!!timeoutDelayError}
														helperText={timeoutDelayError}
														fullWidth/>
												</FieldContainer>
												<InputField
													label='Количество особей в начальной популяции'
													value={startPopulationsCount}
													onChange={setStartPopulationsCount}
													errorText={startPopulationsCountError}/>
												<InputField
													label='Количество особей в популяции'
													value={individualsInPopulationCount}
													onChange={setIndividualsInPopulationCount}
													errorText={individualsInPopulationCountError}/>
												<InputField
													label='Количество особей к кроссоверу'
													value={crossoverSelectionCount}
													onChange={setCrossoverSelectionCount}
													errorText={crossoverSelectionCountError}/>
												<InputField
													label='Количество точек кроссовера'
													value={crossoverPointsCount}
													onChange={setCrossoverPointsCount}
													errorText={crossoverPointsCountError}/>
												<InputField
													label='Коэффициент отбора к мутации'
													value={mutationCoefficient}
													onChange={setMutationCoefficient}
													errorText={mutationCoefficientError}/>
												<InputField
													label='Количество особей к мутации'
													value={mutationSelectionCount}
													onChange={setMutationSelectionCount}
													errorText={mutationSelectionCountError}/>
												<InputField
													label='Вероятность точночной мутации'
													value={pointedMutationProbability}
													onChange={setPointedMutationProbability}
													errorText={pointedMutationProbabilityError}/>
											</Box>
										)}
									</PlanningStepContainer>
								</PlanningStepContainer>
							)}
						</Box>
					</>
				)}
			</PlanningContianer>
		</PageContainer>
	)
}
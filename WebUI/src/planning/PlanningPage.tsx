import styled from "@emotion/styled";
import { Box, Button, Checkbox, Collapse, Divider, FormControl, FormControlLabel, FormLabel,  List, ListItem, ListItemButton, ListItemIcon, ListItemText, Radio, RadioGroup, Step, StepContent, StepLabel, Stepper, Tab, Tabs, Typography } from "@mui/material";
import { HeaderLabel, PageContainer } from "../common/controls";
import { useCallback, useEffect, useRef, useState } from "react";
import { ListItem as Item } from "../common/Item";
import { LoadingProgress } from "../common/LoadingProgress";
import { getOrdersInfo } from "../orders/ordersClient";
import { getProductionLinesInfo } from "../production-lines/productionLinesClient";
import { Dayjs } from "dayjs";
import { ActionsBar } from "../common/elementControls";
import { TimeWithoutSelectField } from "../common/inputs/TimeField";
import { FieldContainer } from "../production-lines/rules/RuleComponents";
import { useFieldWithValidation } from "../common/useItemField";
import { convertToInt, convertToNumber } from "../utils/number-converters/numberConverter";
import { InputField } from "../common/inputs";
import { getOriginalPlan, planningByBruteforce, planningByGenetic, ProductionPlanInfo, RoutesQueueInfo } from "./planningClient";
import { convertToTimeSpan } from "../production-lines/timespanConverter";
import { validateMaxIterationsCount, validateGenerationsCount, validateMutationCoefficient, validateMutationSelectionCount, validateCrossoverSelectionCount, validateIndividualsInPopulationCount, validateCrossoverPointsCount, validatePointedMutationProbability, validateStartPopulationsCount, validateDegradingGenerationsCount } from "./validations";
import { originalPlanOrderNumbers, originalPlanProductionLinesNames } from "./originalPlanProductionData";
import { steps } from "./steps";
import { Gantt, Task, ViewMode } from "gantt-task-react";
import { convertProductionPlanToTasks } from "./productionPlanConverter";
import "gantt-task-react/dist/index.css";
import 'dayjs/locale/ru';
import { toast } from "react-toastify";
import { DateRangePicker } from "../common/inputs/DateRangePicker";
import { getRouteCoordinates } from "../rote-matrix/routesClient";
import 'leaflet/dist/leaflet.css';
import L from "leaflet";
import { MapContainer, TileLayer, Marker, Popup, Polyline } from 'react-leaflet';
import { TabPanel } from "../common/TabPanel";
import { removeMapFlag } from "../common/mapHelpers";

const defaultPosition: [number, number] = [59.57, 30.19];

type AlgorithmType = 'Bruteforce' | 'Genetic';

enum SolutionTab { AllDicisions, SelectedPlan, OriginalPlan, RoutesMap }

type RouteCoordinates = {
	coordinates: [number, number][],
	distance: number,
}

const formatMinutesToDDHHMMSS = (totalMinutes: number) => {
	const seconds = Math.floor((totalMinutes * 60) % 60);
	const hours = Math.floor((totalMinutes / 60) % 24);
	const days = Math.floor(totalMinutes / (60 * 24));
	const minutes = Math.floor(totalMinutes % 60);

	const formattedDays = days > 0 ? `${days} дней ` : '';
	const formattedHours = hours > 0 || days > 0 ? `${hours.toString().padStart(2, '0')} часов ` : '';
	const formattedMinutes = minutes > 0 || hours > 0 || days > 0 ? `${minutes.toString().padStart(2, '0')} минут ` : '';
	const formattedSeconds = seconds > 0 || minutes > 0 || hours > 0 || days > 0 ? `${seconds.toString().padStart(2, '0')} секунд` : '';

	return `${formattedDays}${formattedHours}${formattedMinutes}${formattedSeconds}`.trim();
}

const factorial = (n: number) => {
	if (n < 0) 
		throw new Error("Факториал не определен для отрицательных чисел.");
	
	if (n === 0) 
		return 1;
	
	let result = 1;
	for (let i = 1; i <= n; i++) 
		result *= i;
	
	return result;
}

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

const PlanningContianer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
});

const GanttContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
	width: '100%',
});

const ActionContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
	marginBottom: '1vw',
	marginTop: '15px',
	marginLeft: '15px',
});

const ActionButton = styled(Button)({
	marginLeft: '20px',
	background: '#1d1b31',
	'&:hover': {
		backgroundColor: '#11101d'
	},
});

const ActionLabel = styled(Typography)({
	color: 'black !important',
	fontSize: '1.3rem',
	marginLeft: '20px',
});

const ColoredTabs = styled(Tabs)({
	'& .MuiTabs-indicator': {
		backgroundColor: '#1d1b31',
	},
	'& .MuiTab-root': {
		'&.Mui-selected': {
			color: '#1d1b31',
			fontWeight: 'bold',
		},
	},
});

export const ListBlock = styled(Box)({
	width: 'max-content',
	borderRight: '1px',
	borderColor: 'black',
	height: '700px',
	'overflowY': 'scroll',
	'::-webkit-scrollbar': {
		width: '10px',
	},
	'::-webkit-scrollbar-thumb': {
		backgroundColor: '#1d1b31',
		borderRadius: '5px',
	},
	'::-webkit-scrollbar-thumb:active': {
		backgroundColor: '#11101d',
	},
	paddingRight: '10px',
});

export const PlanningPage = () => {
	const [activeStep, setActiveStep] = useState(0)
	const [isNextStepDisabled, setIsNextStepDisabled] = useState(false)

	const [functionType, setFunctionType] = useState(0)
	
	const [selectedStartDate, setSelectedStartDate] = useState<Date | null>(null);
	const [selectedEndDate, setSelectedEndDate] = useState<Date | null>(null);

	const [orders, setOrders] = useState<Item[]>()
	const [selectedOrders, setSelectedOrders] = useState<number[]>([])

	const [productionLines, setProductionLines] = useState<Item[]>()
	const [selectedProductionLines, setSelectedProductionLines] = useState<number[]>([])

	const [algorithmType, setAlgorithmType] = useState<AlgorithmType>('Bruteforce')
	const [settingsGenerated, setSettingsGenerated] = useState(false)
	const [timeoutDelay, setTimeoutDelay, timeoutDelayError, setTimeoutDelayError] = useFieldWithValidation<Dayjs | null>(null, () => '')
	const [iterationsCount, setIterationsCount, iterationsCountError, setIterationsCountError] = useFieldWithValidation<string>('', validateMaxIterationsCount)
	
	const [generationsCount, setGenerationsCount, generationsCountError] = useFieldWithValidation<string>('1000', validateGenerationsCount)
	const [degradingGenerationsCount, setDegradingGenerationsCount, degradingGenerationsCountError] = useFieldWithValidation<string>('', validateDegradingGenerationsCount)
	const [mutationCoefficient, setMutationCoefficient, mutationCoefficientError] = useFieldWithValidation<string>('0.3', validateMutationCoefficient)
	const [mutationSelectionCount, setMutationSelectionCount, mutationSelectionCountError] = useFieldWithValidation<string>('25', validateMutationSelectionCount)
	const [crossoverSelectionCount, setCrossoverSelectionCount, crossoverSelectionCountError] = useFieldWithValidation<string>('100', validateCrossoverSelectionCount)
	const [individualsInPopulationCount, setIndividualsInPopulationCount, individualsInPopulationCountError] = useFieldWithValidation<string>('200', validateIndividualsInPopulationCount)
	const [crossoverPointsCount, setCrossoverPointsCount, crossoverPointsCountError] = useFieldWithValidation<string>('2', validateCrossoverPointsCount)
	const [pointedMutationProbability, setPointedMutationProbability, pointedMutationProbabilityError] = useFieldWithValidation<string>('0,35', validatePointedMutationProbability)
	const [startPopulationsCount, setStartPopulationsCount, startPopulationsCountError] = useFieldWithValidation<string>('400', validateStartPopulationsCount)

	const [tabID, setTabID] = useState(SolutionTab.SelectedPlan);

	const [planInfos, setPlanInfos] = useState<ProductionPlanInfo[]>();
	const [routesQueueInfos, setRoutesQueueInfos] = useState<RoutesQueueInfo[]>();
	const [executionTime, setExecutionTime] = useState<number | null>(null);
	const [selectedPlan, setSelectedPlan] = useState<number>();
	const [tasks, setTasks] = useState<Task[]>();
	const [targetFunctionResult, setTargetFunctionResult] = useState<string>('');

	const [isOriginalPlanAvaliable, setIsOriginalPlanAvaliable] = useState(false);
	const [originalPlan, setOriginalPlan] = useState<ProductionPlanInfo>();
	const [originalTasks, setOriginalTasks] = useState<Task[]>();
	const [originalTargetFunctionResult, setOriginalTargetFunctionResult] = useState<string>('');
	
	const [viewMode, setViewMode] = useState<ViewMode>(ViewMode.Day);

	const [routeCoordinates, setRouteCoordinates] = useState<RouteCoordinates[]>();
	const [markerPosition, setMarkerPosition] = useState<[number, number] | null>(null);
	
	const mapRef = useRef<L.Map | null>(null);
	
	const handleNext = async () => {
		setActiveStep((prevActiveStep) => prevActiveStep + 1);
	}
  
	const handleBack = () => {
		setActiveStep((prevActiveStep) => prevActiveStep - 1);
	};

	useEffect(() => {
		if (activeStep === 1 && (!selectedStartDate || !selectedEndDate))
			setIsNextStepDisabled(true);
		else if (activeStep === 2 && selectedOrders.length === 0) 
			setIsNextStepDisabled(true);
		else if (activeStep === 3 && selectedProductionLines.length === 0)
			setIsNextStepDisabled(true);
		else if (activeStep === 4 && algorithmType === 'Bruteforce')
			if (timeoutDelay === null && iterationsCount === '') {
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
					|| startPopulationsCountError
					|| degradingGenerationsCountError) 
				setIsNextStepDisabled(true);

			setTimeoutDelayError('');
			setIterationsCountError('');
			setIsNextStepDisabled(false);
		} else 
			setIsNextStepDisabled(false);
		
	}, [
		activeStep,
		selectedStartDate,
		selectedEndDate,
		selectedOrders,
		selectedProductionLines,
		algorithmType,
		timeoutDelay,
		iterationsCount,
		generationsCountError,
		degradingGenerationsCountError,
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

	const handleDateRangeChange = (startDate: Date | null, endDate: Date | null) => {
		setSelectedStartDate(startDate);
		setSelectedEndDate(endDate);
	};
	
	const loadOrders = useCallback(async () => {
		const items = await getOrdersInfo();      
		setOrders(items);
	}, []);

	const selectOldPlanOrders = () => {
		if (orders !== undefined) {
			const newSelectedOrders = orders.filter(order => originalPlanOrderNumbers.includes(order.name)).map(order => order.id);
			setSelectedOrders(newSelectedOrders);
		}
	};

	const selectOldPlanProductionLines = () => {
		if (productionLines !== undefined) {
			const newSelectedProductionLines = productionLines.filter(line => originalPlanProductionLinesNames.includes(line.name)).map(order => order.id);
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
		if (activeStep === 4 && settingsGenerated === false && selectedOrders && selectedProductionLines) {
			if (selectedOrders.length + selectedProductionLines.length <= 6) {
				setAlgorithmType('Bruteforce');
				const iterationsCount = factorial(selectedOrders.length + selectedProductionLines.length - 1) / factorial(selectedProductionLines.length - 1);
				setIterationsCount((iterationsCount).toFixed(0).toString());
			} else if (selectedOrders.length + selectedProductionLines.length <= 11) {
				setAlgorithmType('Bruteforce');
				const iterationsCount = factorial(selectedOrders.length + selectedProductionLines.length - 1) / factorial(selectedProductionLines.length - 1) * 0.77;
				setIterationsCount((iterationsCount).toFixed(0).toString());
			} else {
				setAlgorithmType('Genetic');
				setGenerationsCount(Math.min(selectedOrders.length * selectedProductionLines.length * 20, 3000).toString());
				const populationsCount = Math.min(selectedOrders.length * selectedProductionLines.length / 0.145, 600);
				setStartPopulationsCount((populationsCount).toFixed(0).toString());
				setIndividualsInPopulationCount((populationsCount / 2).toFixed(0).toString());
				setCrossoverSelectionCount((populationsCount / 4).toFixed(0).toString());
				setCrossoverPointsCount(Math.min(((selectedOrders.length / selectedProductionLines.length) / 19) * selectedProductionLines.length, 10).toFixed(0).toString());
				setMutationSelectionCount((populationsCount / 16).toFixed(0).toString());
			}

			setSettingsGenerated(true);
		}
		
	}, [activeStep, selectedOrders, selectedProductionLines, algorithmType, settingsGenerated]);

	useEffect(() => {
		if (activeStep === steps.length) {
			const tryRunPlanning = async () => {
				if (algorithmType === 'Bruteforce') {
					const iterationsCountNum = convertToInt(iterationsCount);
					if (selectedStartDate && (iterationsCountNum || timeoutDelay !== null)) {
						const startTime = performance.now();
						try {
							const result = await planningByBruteforce({
								startDateTime: selectedStartDate,
								orders: selectedOrders,
								productionLines: selectedProductionLines,
								functionType: functionType,
								conditions: {
									timeoutDelay: timeoutDelay === null ? undefined : convertToTimeSpan(timeoutDelay),
									iterationsCount: iterationsCountNum,
								}
							});
	
							setPlanInfos(result.productionPlans.sort((a, b) => b.targetFunctionValue - a.targetFunctionValue));
							setRoutesQueueInfos(result.routesQueues);
							toast.success('Оптимальный производсвтенный план построен');
						} catch (error: unknown) {
							console.log(`Catch an error while trying planning by bruteforce ${error}`);
							setActiveStep((prevActiveStep) => prevActiveStep - 1);
							toast.error('Не удалось построить производсвтенный план, попробуйте поменять парметры задачи и повторите попытку');
						} finally {
							const endTime = performance.now();
							const timeDiff = (endTime - startTime) / 1000;
							setExecutionTime(timeDiff);
						}
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
					const degradingGenerationsCountNum = convertToInt(degradingGenerationsCount);
	
					if (selectedStartDate
						&& generationsCountNum
						&& mutationCoefficientNum
						&& mutationSelectionCountNum
						&& crossoverSelectionCountNum
						&& individualsInPopulationCountNum
						&& crossoverPointsCountNum
						&& pointedMutationProbabilityNum
						&& startPopulationsCountNum
						&& !degradingGenerationsCountError) {
						const startTime = performance.now();
						try {
							const result = await planningByGenetic({
								startDateTime: selectedStartDate,
								orders: selectedOrders,
								productionLines: selectedProductionLines,
								functionType: functionType,
								conditions: {
									timeoutDelay: timeoutDelay === null ? undefined : convertToTimeSpan(timeoutDelay),
									iterationsCount: generationsCountNum,
									generationsCount: degradingGenerationsCountNum,
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
		
							setPlanInfos(result.productionPlans.sort((a, b) => b.targetFunctionValue - a.targetFunctionValue));
							setRoutesQueueInfos(result.routesQueues);
							toast.success('Оптимальный производсвтенный план построен');
						} catch (error: unknown) {
							console.log(`Catch an error while trying planning by genetic ${error}`);
							setActiveStep((prevActiveStep) => prevActiveStep - 1);
							toast.error('Не удалось построить производсвтенный план, попробуйте поменять парметры задачи и повторите попытку');
						} finally {
							const endTime = performance.now();
							const timeDiff = (endTime - startTime) / 1000;
							setExecutionTime(timeDiff);
						}
					}
					else
						setActiveStep((prevActiveStep) => prevActiveStep - 1);
				}
			}
	
			tryRunPlanning();
		}
	}, [
		activeStep,
		algorithmType,
		selectedOrders,
		selectedProductionLines,
		functionType,
		timeoutDelay,
		generationsCount,
		degradingGenerationsCount,
		degradingGenerationsCountError,
		mutationCoefficient,
		mutationSelectionCount,
		crossoverSelectionCount,
		individualsInPopulationCount,
		crossoverPointsCount,
		pointedMutationProbability,
		startPopulationsCount,
	]);

	useEffect(() => {
		if (planInfos && planInfos.length > 0) 
			setSelectedPlan(planInfos.length - 1);
		
	}, [planInfos]);

	const handleReturnToPlanning = () => {
		setTasks(undefined);
		setOriginalTasks(undefined);
		setPlanInfos(undefined);
		setSelectedPlan(undefined);
		setActiveStep(0);
		setSettingsGenerated(false);
	};

	useEffect(() => {
		if (planInfos && selectedPlan !== undefined) {
			const newTasks = convertProductionPlanToTasks(planInfos[selectedPlan]);
			setTasks(newTasks);

			if (functionType == 0)
				setTargetFunctionResult(formatMinutesToDDHHMMSS(planInfos[selectedPlan].targetFunctionValue));
			else
				setTargetFunctionResult(`${planInfos[selectedPlan].targetFunctionValue.toFixed(2)} у.е.`);
		}
	}, [planInfos, selectedPlan, functionType]);

	useEffect(() => {
		if (functionType !== 0 || !orders || !productionLines || selectedOrders.length !== originalPlanOrderNumbers.length || selectedProductionLines.length !== originalPlanProductionLinesNames.length)
			setIsOriginalPlanAvaliable(false);
		else {
			const originalOrdersIDsSet = new Set(orders.filter(order => originalPlanOrderNumbers.includes(order.name)).map(order => order.id));
			const originalProductionLinesIDs = productionLines.filter(line => originalPlanProductionLinesNames.includes(line.name)).map(order => order.id);

			setIsOriginalPlanAvaliable(selectedOrders.every(element => originalOrdersIDsSet.has(element))
				&& selectedProductionLines.every(element => originalProductionLinesIDs.includes(element)));
		}
	}, [functionType, orders, productionLines, selectedOrders, selectedProductionLines]);

	useEffect(() => {
		if (isOriginalPlanAvaliable && selectedStartDate !== null && originalPlan === undefined) {
			const loadOriginalPlan = async () => {
				try {
					const plan = await getOriginalPlan(selectedStartDate);
					setOriginalPlan(plan);

					toast.success('Оригинальный план загружен');
				} catch {
					toast.error('При загрузке оригинального плана произошла ошибка, проверьте данные производства');
				}
			};

			loadOriginalPlan();
		}
	}, [isOriginalPlanAvaliable, originalPlan, selectedStartDate]);

	useEffect(() => {
		if (originalPlan) {
			const newTasks = convertProductionPlanToTasks(originalPlan);
			setOriginalTasks(newTasks);
			setOriginalTargetFunctionResult(formatMinutesToDDHHMMSS(originalPlan.targetFunctionValue));
		}
	}, [originalPlan]);

	useEffect(() => {
		removeMapFlag();
	}, [tabID]);

	useEffect(() => {
		removeMapFlag();
	}, []);
  
	const handleTabChange = (event: React.SyntheticEvent, newValue: SolutionTab) => {
		setTabID(newValue);
		removeMapFlag();
	};

	useEffect(() => {
		if (routesQueueInfos) {
			const loadRoutesCoordinates = async () => {
				const coordinateCollection: RouteCoordinates[] = [];

				for (const productionRoutes of routesQueueInfos) 
					for (const customerRoute of productionRoutes.customerInfos) 
						if (productionRoutes.productionInfo.entityCoordinates && customerRoute.entityCoordinates) {
							const coordinates = await getRouteCoordinates(productionRoutes.productionInfo.entityCoordinates, customerRoute.entityCoordinates);

							if (coordinates)
								coordinateCollection.push(coordinates);

							if (coordinateCollection.length == 1) 
								setMarkerPosition([
									productionRoutes.productionInfo.entityCoordinates.latitude,
									productionRoutes.productionInfo.entityCoordinates.longitude,
								]);
							
						}

				setRouteCoordinates(coordinateCollection);
			}

			loadRoutesCoordinates();
		}
	}, [routesQueueInfos]);
	
		
	useEffect(() => {
		delete(L.Icon.Default.prototype as any)._getIconUrl;
		L.Icon.Default.mergeOptions({
			iconRetinaUrl:require('leaflet/dist/images/marker-icon-2x.png'),
			iconUrl:require('leaflet/dist/images/marker-icon.png'),
			shadowUrl:require('leaflet/dist/images/marker-shadow.png')}
		)
	}, []);

	return (
		<PageContainer sx={{ height: '100vh' }}>
			<HeaderLabel>Оптимальное планирование</HeaderLabel>
			<PlanningContianer sx={{ overflow: 'auto' }}>
				{ activeStep === steps.length ? (
					<>
						{ planInfos ? (
							<GanttContainer>
								<ActionContainer>
									<ColoredTabs value={tabID} onChange={handleTabChange} >
										<Tab label="Остальные решения" value={SolutionTab.AllDicisions}/>
										<Tab label="План производства" value={SolutionTab.SelectedPlan}/>
										{ isOriginalPlanAvaliable && (
											<Tab label="Оригинальный план" value={SolutionTab.OriginalPlan}/>
										)}
										<Tab label="Карта производств и поставок" value={SolutionTab.RoutesMap}/>
									</ColoredTabs>
									<ActionButton variant="contained" onClick={handleReturnToPlanning}>
										Вернуться к планированию
									</ActionButton>
								</ActionContainer>
								<TabPanel value={tabID} index={SolutionTab.AllDicisions}>
									<PlanningStepContainer>
										<BlackLabel sx={{ marginLeft: '15px' }}>Поэтапный список решений</BlackLabel>
										<List>
											{ planInfos.map((value, index) => (
												<ListItem key={index} disablePadding>
													<ListItemButton role={undefined} onClick={() => setSelectedPlan(index)}>
														<ListItemText primary={`${index + 1} Производственный план (${functionType === 0 ? formatMinutesToDDHHMMSS(value.targetFunctionValue) : `${value.targetFunctionValue.toFixed(2)} у.е.`})`} />
													</ListItemButton>
												</ListItem>
											))}
										</List>
									</PlanningStepContainer>
								</TabPanel>
								{ tasks && (
									<TabPanel value={tabID} index={SolutionTab.SelectedPlan}>
										<ActionContainer>
											<ActionLabel>Значение критерия: {targetFunctionResult}</ActionLabel>
											<ActionLabel>Время вычисленний: {executionTime?.toFixed(2)} сек.</ActionLabel>
										</ActionContainer>
										<Gantt tasks={tasks} viewMode={viewMode}/>
									</TabPanel>
								)}
								{ originalTasks && (
									<TabPanel value={tabID} index={SolutionTab.OriginalPlan}>
										<ActionContainer>
											<ActionLabel>Значение критерия: {originalTargetFunctionResult}</ActionLabel>
										</ActionContainer>
										<Gantt tasks={originalTasks} viewMode={viewMode}/>
									</TabPanel>
								)}
								{ routesQueueInfos && (
									<TabPanel value={tabID} index={SolutionTab.RoutesMap}>
										<Box sx={{ display: 'flex', flexDirection: 'row' }}>
											<ListBlock>
												<List>
													{ routesQueueInfos.map(item => (
															<>
																<ListItemButton>
																	<ListItemText primary={item.productionInfo.entityName}/>
																</ListItemButton>
																<Collapse in={true} timeout="auto" unmountOnExit>
																	<List component="div" disablePadding>
																		{ item.customerInfos.map(item => (
																			<ListItemButton sx={{ pl: 4 }}>
																				<ListItemText primary={item.entityName} />
																			</ListItemButton>
																		))}
																	</List>
																</Collapse>
															</>
													)) }
												</List>
											</ListBlock>
											<MapContainer center={ markerPosition || defaultPosition} zoom={6} style={{ height: '700px', width: '100%' }} ref={mapRef}>
												<TileLayer
													url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
													attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
												/>
												{ routesQueueInfos.map(item => {
													removeMapFlag();
													return (
														<>
															{ item.productionInfo.entityCoordinates && (
																<Marker position={[item.productionInfo.entityCoordinates.latitude, item.productionInfo.entityCoordinates.longitude]}>
																	<Popup>{item.productionInfo.entityName}</Popup>
																</Marker>
															) }
														</>
													)
												})}
												{ routesQueueInfos.map(item => (
													<>
														{ item.customerInfos.map(item => (
															<>
																{ item.entityCoordinates && (
																	<Marker position={[item.entityCoordinates.latitude, item.entityCoordinates.longitude]}>
																		<Popup>{item.entityName}</Popup>
																	</Marker>
																)}
															</>
														)) }
													</>
												))}
												{ routeCoordinates && (
													<>
														{ routeCoordinates.map(item => (
															<>
																{ item.coordinates.length > 0 && (
																	<Polyline positions={item.coordinates} color="blue" />
																)}
															</>
														))}
													</>
												)}
											</MapContainer>
										</Box>
									</TabPanel>
								)}
							</GanttContainer>
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
									<BlackLabel>Выберите метод оптимального планирования</BlackLabel>
									<RadioGroup value={functionType} onChange={handleFunctionTypeChange}>
										<FormControlLabel value={0} control={<Radio />} label="Время производства" />
										<FormControlLabel value={1} control={<Radio />} label="Стоимость производства" />
									</RadioGroup>
								</PlanningStepContainer>
							)}
							{activeStep === 1 && (
								<PlanningStepContainer id='MyDateRangeCalendarPicker'>
									<FormLabel sx={{ color: 'black !important', fontWeight: 'bold', fontSize: '1.3rem' }}>Выберите период планирования</FormLabel>
									<DateRangePicker onDateRangeChange={handleDateRangeChange} />
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
										<List sx={{ width: '100%', maxHeight: 'calc(100vh - 185px)', 'overflowY': 'auto', }}>
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
										<List sx={{ width: '100%', maxHeight: 'calc(100vh - 185px)', 'overflowY': 'auto', }}>
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
													onChange={setGenerationsCount}
													errorText={generationsCountError}/>
												<InputField
													label='Количество деградирующих поколений для остановки'
													value={degradingGenerationsCount}
													onChange={setDegradingGenerationsCount}
													errorText={degradingGenerationsCountError}/>
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
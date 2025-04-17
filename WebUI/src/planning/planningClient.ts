import axios, { AxiosError } from "axios";
import { ClientError } from "../common/clients/clientError";
import { handleError } from "../common/clients/clients";
import { nameof } from "../utils/nameof/nameof";
import { EntityLocationInfo } from "../rote-matrix/routesClient";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'optimization';

export interface PlanningTask {
    startDateTime: Date,
    orders: number[],
    productionLines: number[],
    functionType: number
}

export interface PlanningTaskConditions {
    timeoutDelay: string | undefined,
    iterationsCount: number | undefined,
}

export interface BruteforcePlanningTask extends PlanningTask {
    conditions: PlanningTaskConditions,
}

export interface GeneticPlanningTaskConditions {
    timeoutDelay: string | undefined,
    iterationsCount: number | undefined,
    generationsCount: number | undefined,
}

export interface GeneticOptions {
    mutationCoefficient: number,
    mutationSelectionCount: number,
    crossoverSelectionCount: number,
    individualsInPopulationCount: number,
    crossoverPointsCount: number,
    pointedMutationProbability: number,
    startPopulationsCount: number,
}

export interface GeneticforcePlanningTask extends PlanningTask {
    conditions: GeneticPlanningTaskConditions,
    options: GeneticOptions,
}

export interface OrderPositionInfo {
    orderNumber: string,
    orderProductionStartDateTime: string,
    orderProductionEndDateTime: string,
}

export interface ProductionLineQueueInfo {
    productionLineName: string,
    orderPositions: OrderPositionInfo[],
}

export interface RoutesQueueInfo {
    productionInfo: EntityLocationInfo,
    customerInfos: EntityLocationInfo[],
}

export interface ProductionPlanInfo {
    startDateTime: Date,
    productionLineQueues: ProductionLineQueueInfo[],
    targetFunctionValue: number,
}

export interface PlanningInfo {
    productionPlans: ProductionPlanInfo[],
    routesQueues: RoutesQueueInfo[],
}

export const planningByBruteforce = async (task: BruteforcePlanningTask) => {
	try {
		const { data } = await axios.post<PlanningInfo>(`${API_ROOT}${API_URL}/bruteforce`, task);
		return data;
	} catch (error: unknown) {
		handleError(nameof({planningByBruteforce}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const planningByGenetic = async (task: GeneticforcePlanningTask) => {
	try {
		const { data } = await axios.post<PlanningInfo>(`${API_ROOT}${API_URL}/genetic`, task);
		return data;
	} catch (error: unknown) {
		handleError(nameof({planningByGenetic}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const getOriginalPlan = async (startDateTime: Date) => {
	const formattedDate = startDateTime.toISOString();
	try {
		const { data } = await axios.get<ProductionPlanInfo>(`${API_ROOT}${API_URL}/original-plan?startDateTime=${formattedDate}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getOriginalPlan}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}
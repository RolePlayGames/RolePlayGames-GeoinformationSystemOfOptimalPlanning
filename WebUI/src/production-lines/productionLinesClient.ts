import axios, { AxiosError } from "axios"
import { nameof } from "../utils/nameof/nameof";
import { handleError } from "../common/clients/clients";
import { ClientError } from "../common/clients/clientError";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'production-lines';

export type ProductionLineInfo = {
    id: number,
    name: string,
}

export type ProductionLine = {
	name: string,
	hourCost: number,
	maxProductionSpeed: number,
	widthMin: number,
	widthMax: number,
	thicknessMin: number,
	thicknessMax: number,
	thicknessChangeTime: string,
	thicknessChangeConsumption: number,
	widthChangeTime: string,
	widthChangeConsumption: number,
	setupTime: string,
}

export const getProductionLinesInfo = async () => {
	try {
		const { data } = await axios.get<ProductionLineInfo[]>(`${API_ROOT}${API_URL}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getProductionLinesInfo}), error as AxiosError);
		return undefined;
	}
}

export const getProductionLine = async (id: number) => {
	try {
		const { data } = await axios.get<ProductionLine>(`${API_ROOT}${API_URL}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getProductionLine}), error as AxiosError);
		return undefined;
	}
}

export const createProductionLine = async (order: ProductionLine) => {
	try {
		const { data } = await axios.post<number>(`${API_ROOT}${API_URL}`, order);
		return data;
	} catch (error: unknown) {
		handleError(nameof({createProductionLine}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const updateProductionLine = (id: number, order: ProductionLine) => {
	try {
		return axios.post(`${API_ROOT}${API_URL}/${id}`, order);
	} catch (error: unknown) {
		handleError(nameof({updateProductionLine}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const deleteProductionLine = async (id: number) => {
	try {
		await axios.delete(`${API_ROOT}${API_URL}/${id}`);
		return true;
	} catch (error: unknown) {
		handleError(nameof({deleteProductionLine}), error as AxiosError);
		return false;
	}
}
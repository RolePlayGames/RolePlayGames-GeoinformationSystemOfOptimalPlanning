import axios, { AxiosError } from "axios";
import { ClientError } from "../common/clients/clientError";
import { handleError } from "../common/clients/clients";
import { nameof } from "../utils/nameof/nameof";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const PRODUCTIONS_ROOT = 'productions';

export type ProductionInfo = {
    id: number,
    name: string,
}

export type Coordinates = {
    latitude: number,
    longitude: number,
}

export type Production = {
    name: string,
	coordinates: Coordinates | undefined,
}

export const getProductionsInfo = async () => {
	try {
		const { data } = await axios.get<ProductionInfo[]>(`${API_ROOT}${PRODUCTIONS_ROOT}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getProductionsInfo}), error as AxiosError);
		return undefined;
	}
}

export const getProduction = async (id: number) => {
	try {
		const { data } = await axios.get<Production>(`${API_ROOT}${PRODUCTIONS_ROOT}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getProduction}), error as AxiosError);
		return undefined;
	}
}

export const createProduction = async (customer: Production) => {
	try {
		const { data } = await axios.post<number>(`${API_ROOT}${PRODUCTIONS_ROOT}`, customer);
		return data;
	} catch (error: unknown) {
		handleError(nameof({createProduction}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const updateProduction = (id: number, customer: Production) => {
	try {
		return axios.post(`${API_ROOT}${PRODUCTIONS_ROOT}/${id}`, customer);
	} catch (error: unknown) {
		handleError(nameof({updateProduction}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const deleteProduction = async (id: number) => {
	try {
		await axios.delete(`${API_ROOT}${PRODUCTIONS_ROOT}/${id}`);
		return true;
	} catch (error: unknown) {
		handleError(nameof({deleteProduction}), error as AxiosError);
		return false;
	}
}
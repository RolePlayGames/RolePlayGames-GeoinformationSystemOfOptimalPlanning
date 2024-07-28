import axios, { AxiosError } from "axios"
import { nameof } from "../utils/nameof/nameof";
import { handleError } from "../common/clients/clients";
import { ClientError } from "../common/clients/clientError";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'orders';

export type OrderInfo = {
    id: number,
    name: string,
}

export type Order = {
    number: string,
	customerID: number,
	filmRecipeID: number,
	width: number,
	quantityInRunningMeter: number,
	finishedGoods: number,
	waste: number,
	rollsCount: number,
	plannedDate: Date,
	priceOverdue: number,
}

export const getOrdersInfo = async () => {
	try {
		const { data } = await axios.get<OrderInfo[]>(`${API_ROOT}${API_URL}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getOrdersInfo}), error as AxiosError);
		return undefined;
	}
}

export const getOrder = async (id: number) => {
	try {
		const { data } = await axios.get<Order>(`${API_ROOT}${API_URL}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getOrder}), error as AxiosError);
		return undefined;
	}
}

export const createOrder = async (order: Order) => {
	try {
		const { data } = await axios.post<number>(`${API_ROOT}${API_URL}`, order);
		return data;
	} catch (error: unknown) {
		handleError(nameof({createOrder}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const updateOrder = (id: number, order: Order) => {
	try {
		return axios.post(`${API_ROOT}${API_URL}/${id}`, order);
	} catch (error: unknown) {
		handleError(nameof({updateOrder}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const deleteOrder = async (id: number) => {
	try {
		await axios.delete(`${API_ROOT}${API_URL}/${id}`);
		return true;
	} catch (error: unknown) {
		handleError(nameof({deleteOrder}), error as AxiosError);
		return false;
	}
}
import axios, { AxiosError } from "axios"
import { toast } from "react-toastify";
import { nameof } from "../utils/nameof/nameof";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const CUSTOMERS_ROOT = 'customers';

const handleError = (methodName: string, error: AxiosError | undefined) => {
    if (error) {
        console.log(`${methodName} получил ответ ${error.response?.status} с сообщением ${error.response?.data} and error: ${error}`);
        //toast.error('Что-то пошло не так, попробуйте позже');
    }
}

export type CustomerInfo = {
    id: number,
    name: string,
}

export type Customer = {
    name: string,
}

export const getCustomersInfo = async () => {
    try {
        const { data } = await axios.get<CustomerInfo[]>(`${API_ROOT}${CUSTOMERS_ROOT}/info`);
        return data;
    } catch (error: unknown) {
        handleError(nameof({getCustomersInfo}), error as AxiosError);
        return undefined;
	}
}

export const getCustomer = async (id: number) => {
    try {
        const { data } = await axios.get<Customer>(`${API_ROOT}${CUSTOMERS_ROOT}/${id}`);
        return data;
    } catch (error: unknown) {
        handleError(nameof({getCustomer}), error as AxiosError);
        return undefined;
	}
}

export const createCustomer = async (customer: Customer) => {
    try {
        const { data } = await axios.post<number>(`${API_ROOT}${CUSTOMERS_ROOT}`, customer);
        return data;
    } catch (error: unknown) {
        handleError(nameof({createCustomer}), error as AxiosError);
        throw new ClientError(error as AxiosError);
	}
}

export const updateCustomer = (id: number, customer: Customer) => {
    try {
        return axios.post(`${API_ROOT}${CUSTOMERS_ROOT}/${id}`, customer);
    } catch (error: unknown) {
        handleError(nameof({updateCustomer}), error as AxiosError);
        throw new ClientError(error as AxiosError);
	}
}

export const deleteCustomer = async (id: number) => {
    try {
        await axios.delete(`${API_ROOT}${CUSTOMERS_ROOT}/${id}`);
        return true;
    } catch (error: unknown) {
        handleError(nameof({updateCustomer}), error as AxiosError);
        return false;
	}
}

export interface IClientError {
    status: number;
	errorCode: string;
}

export class ClientError extends Error implements IClientError {
    status: number;
	errorCode: string;
	constructor(error: AxiosError) {
		super(error.message);

        this.status = error.response?.status as number;
        this.errorCode = error.response?.data as string;
	}
}
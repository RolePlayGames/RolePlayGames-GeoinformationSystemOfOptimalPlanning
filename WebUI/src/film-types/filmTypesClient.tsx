import axios, { AxiosError } from "axios"
import { toast } from "react-toastify";
import { nameof } from "../utils/nameof/nameof";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'film-types';

const handleError = (methodName: string, error: AxiosError | undefined) => {
	if (error) 
		console.log(`${methodName} получил ответ ${error.response?.status} с сообщением ${error.response?.data} and error: ${error}`);
	//toast.error('Что-то пошло не так, попробуйте позже');
    
}

export type FilmTypeInfo = {
    id: number,
    name: string,
}

export type FilmType = {
    article: string,
}

export const getFilmTypesInfo = async () => {
	try {
		const { data } = await axios.get<FilmTypeInfo[]>(`${API_ROOT}${API_URL}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getFilmTypesInfo}), error as AxiosError);
		return undefined;
	}
}

export const getFilmType = async (id: number) => {
	try {
		const { data } = await axios.get<FilmType>(`${API_ROOT}${API_URL}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getFilmType}), error as AxiosError);
		return undefined;
	}
}

export const createFilmType = async (filmType: FilmType) => {
	try {
		const { data } = await axios.post<number>(`${API_ROOT}${API_URL}`, filmType);
		return data;
	} catch (error: unknown) {
		handleError(nameof({createFilmType}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const updateFilmType = (id: number, filmType: FilmType) => {
	try {
		return axios.post(`${API_ROOT}${API_URL}/${id}`, filmType);
	} catch (error: unknown) {
		handleError(nameof({updateFilmType}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const deleteFilmType = async (id: number) => {
	try {
		await axios.delete(`${API_ROOT}${API_URL}/${id}`);
		return true;
	} catch (error: unknown) {
		handleError(nameof({deleteFilmType}), error as AxiosError);
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
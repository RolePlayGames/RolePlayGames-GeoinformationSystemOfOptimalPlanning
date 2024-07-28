import axios, { AxiosError } from "axios"
import { nameof } from "../utils/nameof/nameof";
import { handleError } from "../common/clients/clients";
import { ClientError } from "../common/clients/clientError";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'film-types';

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
import axios, { AxiosError } from "axios"
import { nameof } from "../utils/nameof/nameof";
import { handleError } from "../common/clients/clients";
import { ClientError } from "../common/clients/clientError";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'film-recipes';

export type FilmRecipeInfo = {
    id: number,
    name: string,
}

export type FilmRecipe = {
    name: string,
	filmTypeID: number,
	thickness: number,
	productionSpeed: number,
	materialCost: number,
	nozzle: number,
	calibration: number,
	coolingLip: number,
}

export type AvaliableFilmType = {
    id: number,
    name: string,
}

export const getFilmRecipesInfo = async () => {
	try {
		const { data } = await axios.get<FilmRecipeInfo[]>(`${API_ROOT}${API_URL}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getFilmRecipesInfo}), error as AxiosError);
		return undefined;
	}
}

export const getFilmRecipe = async (id: number) => {
	try {
		const { data } = await axios.get<FilmRecipe>(`${API_ROOT}${API_URL}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getFilmRecipe}), error as AxiosError);
		return undefined;
	}
}

export const createFilmRecipe = async (filmType: FilmRecipe) => {
	try {
		const { data } = await axios.post<number>(`${API_ROOT}${API_URL}`, filmType);
		return data;
	} catch (error: unknown) {
		handleError(nameof({createFilmRecipe}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const updateFilmRecipe = (id: number, filmType: FilmRecipe) => {
	try {
		return axios.post(`${API_ROOT}${API_URL}/${id}`, filmType);
	} catch (error: unknown) {
		handleError(nameof({updateFilmRecipe}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const deleteFilmRecipe = async (id: number) => {
	try {
		await axios.delete(`${API_ROOT}${API_URL}/${id}`);
		return true;
	} catch (error: unknown) {
		handleError(nameof({deleteFilmRecipe}), error as AxiosError);
		return false;
	}
}

export const getAvaliableFilmTypes = async () => {
	try {
		const { data } = await axios.get<AvaliableFilmType[]>(`${API_ROOT}${API_URL}/avaliable-film-types`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getAvaliableFilmTypes}), error as AxiosError);
		return undefined;
	}
}
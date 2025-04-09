import axios, { AxiosError } from "axios";
import { ClientError } from "../common/clients/clientError";
import { handleError } from "../common/clients/clients";
import { nameof } from "../utils/nameof/nameof";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const ROUTES_ROOT = 'routes';

export type RouteInfo = {
    id: number,
    name: string,
}

export type Coordinates = {
    latitude: number,
    longitude: number,
}

export type EntityLocationInfo = {
    entityID: number,
    entityName: string,
    entityCoordinates: Coordinates | undefined,
}

export type RouteReadModel = {
    productionInfo: EntityLocationInfo,
    customerInfo: EntityLocationInfo,
    price: number,
    drivingTime: string,
}

export type RouteWriteModel = {
    price: number,
    drivingTime: string,
}

export const getRoutesInfo = async () => {
	try {
		const { data } = await axios.get<RouteInfo[]>(`${API_ROOT}${ROUTES_ROOT}/info`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getRoutesInfo}), error as AxiosError);
		return undefined;
	}
}

export const getRoute = async (id: number) => {
	try {
		const { data } = await axios.get<RouteReadModel>(`${API_ROOT}${ROUTES_ROOT}/${id}`);
		return data;
	} catch (error: unknown) {
		handleError(nameof({getRoute}), error as AxiosError);
		return undefined;
	}
}

export const updateRouter = (id: number, router: RouteWriteModel) => {
	try {
		return axios.post(`${API_ROOT}${ROUTES_ROOT}/${id}`, router);
	} catch (error: unknown) {
		handleError(nameof({updateRouter}), error as AxiosError);
		throw new ClientError(error as AxiosError);
	}
}

export const getRouteCoordinates = (productionCoordinates: Coordinates, customerCoordinates: Coordinates) => {
	if (window.location.protocol === "https:")
		return getRouteCoordinatesByOpenRouteService(productionCoordinates, customerCoordinates);
	else
		return getRouteCoordinatesByOSRM(productionCoordinates, customerCoordinates);
}

export const getRouteCoordinatesByOSRM = async (productionCoordinates: Coordinates, customerCoordinates: Coordinates) => {
	const osrmUrl = `http://router.project-osrm.org/route/v1/car/${productionCoordinates.longitude},${productionCoordinates.latitude};${customerCoordinates.longitude},${customerCoordinates.latitude}?geometries=geojson`;
            
	try {
		const response = await fetch(osrmUrl);
		const data = await response.json();
    
		if (data.code === 'Ok') 
			return {
				coordinates: data.routes[0].geometry.coordinates.map((coord: number[]) => [coord[1], coord[0]]) as [number, number][],
				distance: data.routes[0].distance / 1000,
			};
		else {
			console.error('Ошибка OSRM:', data.message);
			return undefined;
		}        
	} catch (error) {
		console.error('Ошибка при получении маршрута OSRM:', error);
		return undefined;
	}
}

export const getRouteCoordinatesByOpenRouteService = async (productionCoordinates: Coordinates, customerCoordinates: Coordinates) => {
	const orsApiKey = '5b3ce3597851110001cf624837aabc25f4d449be94e68c45fe188343';
	const orsUrl = `https://api.openrouteservice.org/v2/directions/driving-car?api_key=${orsApiKey}&start=${productionCoordinates.longitude},${productionCoordinates.latitude}&end=${customerCoordinates.longitude},${customerCoordinates.latitude}`;

	try {
		const response = await fetch(orsUrl);
		const data = await response.json();

		if (data.features && data.features.length > 0) 
			return {
				coordinates: data.features[0].geometry.coordinates.map((coord: number[]) => [coord[1], coord[0]]) as [number, number][],
				distance: data.features[0].properties.segments[0].distance / 1000,
			};
		else
			console.error('Ошибка OpenRouteService:', data);
      
	} catch (error) {
		console.error('Ошибка при получении маршрута:', error);
	}
}
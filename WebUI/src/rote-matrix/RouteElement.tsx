import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton } from "../common/elementControls";
import { InputField } from "../common/inputs";
import { useItemFieldWithValidation, useItemField } from "../common/useItemField";
import { getRouteCoordinates, RouteReadModel, RouteWriteModel, updateRouter } from "./routesClient";
import { Dayjs } from "dayjs";
import { convertFromTimeSpan, convertToTimeSpan } from "../production-lines/timespanConverter";
import { convertToNumber } from "../utils/number-converters/numberConverter";
import { TimeWithoutSelectField } from "../common/inputs/TimeField";
import { useCallback, useEffect, useRef, useState } from "react";
import L from "leaflet";
import { MapContainer, TileLayer, Marker, Popup, Polyline } from 'react-leaflet';
import { styled, Typography } from "@mui/material";
import 'leaflet/dist/leaflet.css';

const validatePrice = (price: string) => {
	const number = convertToNumber(price);

	if (number === undefined)
		return 'Введите число';

	if (number < 0)
		return 'Стоимость должна быть неотрицательной';

	return undefined;
}

const removeMapFlag = () => {
	const link = document.querySelector('a[title="A JavaScript library for interactive maps"]');

	if (link) {
		const svgElement = link.querySelector('svg');

		if (svgElement)
			svgElement.remove();
	}
};

const DistanceLabel = styled(Typography)({
	alignContent: 'center',
	fontSize: 'larger',
	fontWeight: 'bold',
});

type RouteElementProps = {
    id: number,
    item: RouteReadModel,
    apiPath: string,
}

export const RouteElement = ({ id, item, apiPath }: RouteElementProps) => {
	const [price, setPrice, priceError,] = useItemFieldWithValidation<RouteReadModel, string>(item, x => x.price.toString(), validatePrice);
	const [drivingTime, setDrivingTime] = useItemField<RouteReadModel, Dayjs | null>(item, x => convertFromTimeSpan(x.drivingTime));
    
	const [mapCenter, setMapCenter] = useState<[number, number]>();
	const [routeCoordinates, setRouteCoordinates] = useState<[number, number][]>([]);
	const [routeDistance, setRouteDistance] = useState<number>();

	const mapRef = useRef<L.Map | null>(null);

	const navigate = useNavigate();

	const routeName = `${item.productionInfo.entityName} - ${item.customerInfo.entityName}`;

	useEffect(() => {
		if (item.productionInfo.entityCoordinates && item.customerInfo.entityCoordinates) {
			const center: [number, number] = [
				(item.productionInfo.entityCoordinates.latitude + item.customerInfo.entityCoordinates.latitude) / 2,
				(item.productionInfo.entityCoordinates.longitude + item.customerInfo.entityCoordinates.longitude) / 2,
			];
	
			setMapCenter(center);
		} else 
			setMapCenter(undefined);
		
	}, [item]);

	useEffect(() => {
		if (item.productionInfo.entityCoordinates && item.customerInfo.entityCoordinates) {
			const getRoute = async () => {
				const coordinates = await getRouteCoordinates(item.productionInfo.entityCoordinates!, item.customerInfo.entityCoordinates!);
			
				if (coordinates) {
					setRouteCoordinates(coordinates.coordinates);
					setRouteDistance(coordinates.distance);
				
					// Центрируем карту по маршруту
					if (mapRef.current) {
						const bounds = L.latLngBounds(coordinates.coordinates);
						mapRef.current.fitBounds(bounds);
					}
				} else
					toast.error('Неудалось построить маршрут между точками, попробуйте позже.');
			};
		
			getRoute();
		} else {
			setRouteCoordinates([]);
			setRouteDistance(undefined);
		}
	}, [item]);

	const onUpdate = async (item: RouteWriteModel) => {
		try {
			await updateRouter(id, item);
			navigate(apiPath);
			toast.success(`Маршрут ${routeName} был обновлен`);
		} catch (error: unknown) {
			toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);            
		}
	};
    
	const onSave = useCallback(() => {
		const priceNumber = convertToNumber(price);

		if (priceNumber !== undefined && drivingTime !== null)
		{
			const item = {
				price: priceNumber,
				drivingTime: convertToTimeSpan(drivingTime),
			};

			return onUpdate(item);
		}
        
		return Promise.resolve();
	}, [price, drivingTime]);

	useEffect(() => {
		removeMapFlag();
	}, []);

	useEffect(() => {
		removeMapFlag();
	}, [routeCoordinates]);
    
	useEffect(() => {
		delete(L.Icon.Default.prototype as any)._getIconUrl;
		L.Icon.Default.mergeOptions({
			iconRetinaUrl:require('leaflet/dist/images/marker-icon-2x.png'),
			iconUrl:require('leaflet/dist/images/marker-icon.png'),
			shadowUrl:require('leaflet/dist/images/marker-shadow.png')}
		)
	}, []);

	return(
		<ElementContainer>
			<HeaderLabel>Маршрут {routeName}</HeaderLabel>
			<ActionsBar>
				<SaveButton onClick={onSave} disabled={!!priceError}/>
				{routeDistance && (
					<DistanceLabel>{routeDistance.toFixed(3)} км</DistanceLabel>
				)}
			</ActionsBar>
			<InputField
				label='Цена проезда по маршруту'
				value={price}
				onChange={setPrice}
				errorText={priceError}/>
			<TimeWithoutSelectField
				label='Время проезда по маршруту'
				value={drivingTime}
				onChange={setDrivingTime}/>
			{item.productionInfo.entityCoordinates && item.customerInfo.entityCoordinates && (
				<MapContainer center={mapCenter} zoom={6} style={{ height: '500px', width: '100%' }} ref={mapRef}>
					<TileLayer
						url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
						attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
					/>            
					<Marker position={[item.productionInfo.entityCoordinates!.latitude, item.productionInfo.entityCoordinates!.longitude]}>
						<Popup>{item.productionInfo.entityName}</Popup>
					</Marker>            
					<Marker position={[item.customerInfo.entityCoordinates!.latitude, item.customerInfo.entityCoordinates!.longitude]}>
						<Popup>{item.customerInfo.entityName}</Popup>
					</Marker>            
					{routeCoordinates.length > 0 && (
						<Polyline positions={routeCoordinates} color="blue" />
					)}
				</MapContainer>
			)}
		</ElementContainer>
	);
}
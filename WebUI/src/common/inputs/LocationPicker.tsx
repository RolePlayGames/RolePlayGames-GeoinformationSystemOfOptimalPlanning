import { useState, useEffect, useCallback } from 'react';
import { MapContainer, TileLayer, Marker, useMapEvents } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import axios from 'axios';
import { Box } from '@mui/material';
import { InputField } from './inputs';

const defaultZoom = 13;
const defaultPosition: [number, number] = [59.57, 30.19];

const removeMapFlag = () => {
	const link = document.querySelector('a[title="A JavaScript library for interactive maps"]');

	if (link) {
		const svgElement = link.querySelector('svg');

		if (svgElement)
			svgElement.remove();
	}
};

export interface LocationPickerProps {
    initialLatitude?: number;
    initialLongitude?: number;
    onLocationChange: (latitude: number | undefined, longitude: number | undefined) => void;
}

export const LocationPicker: React.FC<LocationPickerProps> = ({
	initialLatitude,
	initialLongitude,
	onLocationChange,
}) => {
	const [address, setAddress] = useState<string | undefined>(undefined);
	const [latitude, setLatitude] = useState<number | undefined>(initialLatitude || undefined);
	const [longitude, setLongitude] = useState<number | undefined>(initialLongitude || undefined);

	const [map, setMap] = useState<L.Map | null>(null);
	const [markerPosition, setMarkerPosition] = useState<[number, number] | null>(initialLatitude && initialLongitude ? [initialLatitude, initialLongitude] : null);

	useEffect(() => {
		removeMapFlag();
	}, []);

	useEffect(() => {
		removeMapFlag();
	}, [address]);

	const reverseGeocode = useCallback(async (lat: number, lon: number) => {
		try {
			const nominatimUrl = `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lon}`;
			const response = await axios.get(nominatimUrl);
			const data = response.data;

			if (data && data.display_name) {
				setAddress(data.display_name);
				return data.display_name;
			} else 
				console.warn("Address not found for coordinates:", lat, lon);
			
		} catch (error) {
			console.error("Error during reverse geocoding:", error);
		}

		setAddress(undefined);
		return null;
	}, []);

	useEffect(() => {
		removeMapFlag();
		async function fetchLocation() {
			if (initialLatitude && initialLongitude) {
				setLatitude(initialLatitude);
				setLongitude(initialLongitude);
				setMarkerPosition([initialLatitude, initialLongitude]);
				reverseGeocode(initialLatitude, initialLongitude);
			} else 
				try {
					const response = await axios.get('https://ipapi.co/json/');
					const { latitude, longitude, city } = response.data;
					setLatitude(latitude);
					setLongitude(longitude);
					setMarkerPosition([latitude, longitude]);
					setAddress(city);
				} catch (error) {
					console.error('Error fetching location:', error);
					console.warn('Could not determine location. Defaulting to Warsaw.');
					setLatitude(defaultPosition[0]);
					setLongitude(defaultPosition[1]);
					setMarkerPosition(defaultPosition);
					reverseGeocode(defaultPosition[0], defaultPosition[1]);
				}
		}
		fetchLocation();

	}, [initialLatitude, initialLongitude, reverseGeocode, defaultPosition]);

	const geocodeAddress = useCallback(async (addr: string) => {
		if (!addr)
			return;
        
		try {
			const nominatimUrl = `https://nominatim.openstreetmap.org/search?q=${encodeURIComponent(addr)}&format=jsonv2`;
			const response = await axios.get(nominatimUrl);
			const data = response.data;

			if (data && data.length > 0) {
				const lat = parseFloat(data[0].lat);
				const lon = parseFloat(data[0].lon);

				setLatitude(lat);
				setLongitude(lon);
				setMarkerPosition([lat, lon]);

				return [lat, lon];
			} else 
				console.warn(`Coordinates not found for address: ${addr}`);
			
		} catch (error) {
			console.error("Error during geocoding:", error);
		}

		setLatitude(undefined);
		setLongitude(undefined);
		setMarkerPosition(null);

		return null;
	}, []);

	const handleAddressChange = (value: string) => {
		setAddress(value);
	};

	const handleAddressSubmit = async (event: React.FormEvent) => {
		event.preventDefault();
		if (address) 
			geocodeAddress(address);        
	};

	const MapEvents = () => {
		const map = useMapEvents({
			click: (e) => {
				const { lat, lng } = e.latlng;
				setLatitude(lat);
				setLongitude(lng);
				setMarkerPosition([lat, lng]);
				reverseGeocode(lat, lng);
				onLocationChange(lat, lng);
			},
		});

		setMap(map);

		return null;
	}

	useEffect(() => {
		if (latitude && longitude) 
			onLocationChange(latitude, longitude);
		else 
			onLocationChange(undefined, undefined);
        
	}, [latitude, longitude, address, onLocationChange]);
                    
	useEffect(() => {
		if(markerPosition)
			map?.setView(markerPosition, defaultZoom)
	}, [map, markerPosition]);

	useEffect(() => {
		delete(L.Icon.Default.prototype as any)._getIconUrl;
		L.Icon.Default.mergeOptions({
			iconRetinaUrl:require('leaflet/dist/images/marker-icon-2x.png'),
			iconUrl:require('leaflet/dist/images/marker-icon.png'),
			shadowUrl:require('leaflet/dist/images/marker-shadow.png')}
		)
	}, []);

	return( 
		<Box component='form' onSubmit={handleAddressSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
			<InputField label='Адрес' value={address || ''} onChange={handleAddressChange} errorText={undefined}/>
			<MapContainer center={ markerPosition || defaultPosition} zoom={defaultZoom} style={{ height: '500px', width: '100%' }} >
				<TileLayer url='https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png' attribution='&copy; OpenStreetMap contributors'/>
				{ markerPosition && <Marker position={markerPosition}/> }
				<MapEvents/>
			</MapContainer>
		</Box>
	);
}
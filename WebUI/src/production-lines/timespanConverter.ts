import dayjs, { Dayjs } from "dayjs";

export const defaultTimeSpan = '00:00:00';

export const convertFromTimeSpan = (value: string) => {
	// Assuming timespanString is in the format "HH:mm:ss" (e.g., "02:30:15")
	const [hours, minutes, seconds] = value.split(':').map(Number);
	return dayjs().hour(hours).minute(minutes).second(seconds);
}

export const convertToTimeSpan = (value: Dayjs) => {
	const hours = value.hour().toString().padStart(2, '0');
	const minutes = value.minute().toString().padStart(2, '0');
	const seconds = value.second().toString().padStart(2, '0');
	return `${hours}:${minutes}:${seconds}`;
}
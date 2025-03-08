import { useState } from 'react';
import { DateCalendar } from '@mui/x-date-pickers/DateCalendar';
import { Dayjs } from 'dayjs';
import { Box, Divider, Typography } from '@mui/material';

export interface DateRangePickerProps {
    onDateRangeChange: (startDate: Date | null, endDate: Date | null) => void;
}

export const DateRangePicker = ({ onDateRangeChange }: DateRangePickerProps) => {
	const [startDate, setStartDate] = useState<Dayjs | null>(null);
	const [endDate, setEndDate] = useState<Dayjs | null>(null);
	const [errorMessage, setErrorMessage] = useState<string | null>(null);

	const handleStartDateChange = (date: Dayjs | null) => {
		setStartDate(date);
		validateDates(date, endDate);
	};

	const handleEndDateChange = (date: Dayjs | null) => {
		setEndDate(date);
		validateDates(startDate, date);
	};

	const validateDates = (start: Dayjs | null, end: Dayjs | null) => {
		if (start && end && start.isAfter(end)) {
			setErrorMessage('Начальная дата должна быть раньше или равна конечной дате.');
			onDateRangeChange(null, null);
		} else {
			setErrorMessage(null);
			const jsStartDate = start ? start.toDate() : null;
			const jsEndDate = end ? end.toDate() : null;
			onDateRangeChange(jsStartDate, jsEndDate);
		}
	};

	return (
		<>
			<Box sx={{ display: 'flex', alignItems: 'center', marginTop: '20px' }}>
				<Box>
					<Typography variant="subtitle1" sx={{ textAlign: 'center' }}>Начальная дата</Typography>
					<DateCalendar value={startDate} onChange={handleStartDateChange} />
				</Box>
				<Divider orientation="vertical" flexItem sx={{ mx: 2 }} />
				<Box>
					<Typography variant="subtitle1" sx={{ textAlign: 'center' }}>Конечная дата</Typography>
					<DateCalendar value={endDate} onChange={handleEndDateChange} />
				</Box>
			</Box>
			{errorMessage && (<Typography color="error" variant="caption" sx={{ textAlign: 'center' }}>{errorMessage}</Typography>)}
		</>
	);
};

import styled from "@emotion/styled";
import { DateTimePicker, DateTimePickerProps } from "@mui/x-date-pickers";
import { Dayjs } from "dayjs";
import { inputFieldStyle } from "./inputFieldStyle";

export type DateTimeFieldProps = DateTimePickerProps<Dayjs, false>

export const DateTimeField = styled((props: DateTimeFieldProps) => (
	<DateTimePicker
		format='DD.MM.YYYY hh:mm'
		slotProps={{
			textField: {
				variant: 'standard',
			},
		}}
		{...props}
	/>
))(inputFieldStyle);
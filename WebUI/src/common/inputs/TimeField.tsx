import styled from "@emotion/styled";
import { TimeField, TimeFieldProps } from "@mui/x-date-pickers";
import { Dayjs } from "dayjs";
import { inputFieldStyle } from "./inputFieldStyle";

export const TimeWithoutSelectField = styled((props: TimeFieldProps<Dayjs, true>) => (
	<TimeField
		format='HH:mm:ss'
		slotProps={{
			textField: {
				variant: 'standard',
			},
		}}
		{...props}
	/>
))(inputFieldStyle);
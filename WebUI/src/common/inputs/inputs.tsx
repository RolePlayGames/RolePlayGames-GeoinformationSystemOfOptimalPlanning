import styled from "@emotion/styled";
import { TextFieldProps, TextField, FormControl } from "@mui/material";
import { PasswordInputProps, PasswordInput } from "./PasswordInput";

export const inputFieldStyle = {
	margin: '10px 0',
	'& label': {
		color: '#11101d',
		fontFamily: 'Open Sans, sans-serif',
	},
	'& label.Mui-focused': {
		color: '#11101d',
	},
	'& .MuiInput-underline:after': {
		borderBottomColor: '#1d1b31',
	},
	'& .MuiOutlinedInput-root': {
		'& fieldset': {
			borderColor: '#1d1b31',
		},
		'&:hover fieldset': {
			borderColor: '#1d1b31',
		},
		'&.Mui-focused fieldset': {
			borderColor: '#1d1b31',
		},
	},
};

export const CommonInputField = styled((props: TextFieldProps) => (
	<TextField
		variant='standard'
		fullWidth
		{...props}
	/>
))(inputFieldStyle);

export const PasswordInputField = (props: PasswordInputProps) => (
	<FormControl fullWidth sx={inputFieldStyle}>
		<PasswordInput {...props}/>
	</FormControl>
);
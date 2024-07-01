import styled from "@emotion/styled";
import { TextFieldProps, TextField, FormControl, InputLabel, Select, SelectProps } from "@mui/material";
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

export type InputFieldProps = {
	label: string,
	value: string,
	onChange: (value: string) => void,
	errorText: string | undefined,
}

export const InputField = ({ label, value, onChange, errorText }: InputFieldProps) => {

	const changeValue = (event: React.ChangeEvent<HTMLInputElement>) => {
		onChange(event.target.value);
	};

	return(
		<CommonInputField
			label={label}
			value={value}
			onChange={changeValue}
			error={!!errorText}
			helperText={errorText}
			sx={{
				marginTop: '1vw',
				marginBottom: '1vw',
			}}/>
	);
}

const SelectContainer = styled(FormControl)({
	margin: '10px 0',
	width: '100%',
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
});

export const SelectField = (props: SelectProps) => (
	<SelectContainer>
		<InputLabel>{props.label}</InputLabel>
		<Select	{...props}/>
	</SelectContainer>
);
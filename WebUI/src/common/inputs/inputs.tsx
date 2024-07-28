import styled from "@emotion/styled";
import { TextFieldProps, TextField, FormControl, InputLabel, Select, SelectProps, SelectVariants, BaseSelectProps } from "@mui/material";
import { PasswordInputProps, PasswordInput } from "./PasswordInput";
import { inputFieldStyle } from "./inputFieldStyle";

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
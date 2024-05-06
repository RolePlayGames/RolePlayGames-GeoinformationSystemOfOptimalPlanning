import { VisibilityOff, Visibility } from "@mui/icons-material";
import { FormHelperText, IconButton, InputAdornment, InputLabel, OutlinedInput, OutlinedInputProps } from "@mui/material";
import { useState, Fragment } from "react";

export interface PasswordInputProps extends OutlinedInputProps {
	errorText?: string;
}

export const PasswordInput = (props: PasswordInputProps) => {
	const [showIcon, setShowIcon] = useState(false);
	const [showPassword, setShowPassword] = useState(false);
	
	const handleClickShowPassword = () => setShowPassword((show) => !show);
	
	const handleVisibilityIconMouseDown = (event: React.MouseEvent<HTMLButtonElement>) => {
		event.preventDefault();
	};

	const passwordChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
		setShowIcon(event.target.value.length > 0);
		props.onChange?.call(this, event);
	};

	return (
		<Fragment>
			<InputLabel htmlFor={props.id} error={props.error}>{props.label}</InputLabel>
			<OutlinedInput
				id={props.id}
				type={showPassword ? 'text' : 'password'}
				endAdornment={
					<InputAdornment position="end">
						{showIcon && (
							<IconButton
								aria-label="toggle password visibility"
								onClick={handleClickShowPassword}
								onMouseDown={handleVisibilityIconMouseDown}
								edge="end"
							>
								{showPassword ? <VisibilityOff/> : <Visibility/>}
							</IconButton>
						)}
					</InputAdornment>
				}
				{...props}
				onChange={passwordChangeHandler}
			/>
			{props.error && (
				<FormHelperText error id={`${props.id}-error`}>
					{props.errorText}
				</FormHelperText>
			)}
		</Fragment>
	);
}
import { InputLabel, Select, SelectProps, FormControl, styled } from "@mui/material";
import { MenuItem, SelectChangeEvent } from "@mui/material";
import { ReactNode } from "react";

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

export type ItemInfo = {
    id: number,
    name: string,
}

export type SelectFieldProps = SelectProps<number | 'standard'>  & {
	items: ItemInfo[],
	onItemChanged: (id: number | undefined) => void,
}

export const SelectField = (props: SelectFieldProps) => {

	const changeItem = (event: SelectChangeEvent<unknown>, child: ReactNode) => {
		const newId = Number(event?.target?.value);
		props.onItemChanged(newId);
	};
	
	return (
		<SelectContainer>
			<InputLabel>{props.label}</InputLabel>
			<Select 
				onChange={changeItem}
				defaultValue={props.items.length > 0 ? props.items[0].id : undefined}
				{...props}
			>
				{props.items.map((item) => 
					<MenuItem 
						key={item.id}
						value={item.id}
					>
						{item.name}
					</MenuItem>
				)}
			</Select>
		</SelectContainer>
	);
};
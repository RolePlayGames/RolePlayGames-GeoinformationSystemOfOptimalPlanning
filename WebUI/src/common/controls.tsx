import styled from "@emotion/styled";
import { Box, Typography, ListItemButton, ButtonProps, Button, List } from "@mui/material";
import AddCircleIcon from '@mui/icons-material/AddCircle';
import React from "react";

export const PageContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
});

export const HeaderLabel = styled(Typography)({
	marginLeft: '1vw',
	padding: '8px',
	color: '#11101d',
	fontSize: '2rem',
	fontFamily: 'Poppins, sans-serif',
	fontWeight: '600',
});

export const MenuItemButton = styled(ListItemButton)({
	minHeight: 48,
	px: 2.5,
});

export const ItemsContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
});

export const ItemsBlock = styled(Box)({
	width: 'max-content',
	borderRight: '1px',
	borderColor: 'black',
});

export const AddItemButton = (props: ButtonProps) => (
	<Button
		variant="contained"
		endIcon={<AddCircleIcon/>}
		sx={{
			marginLeft: '1vw',
			marginRight: '1vw',
			marginTop: '2px',
			marginBottom: '2px',
			width: 'fill-available',
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			},
		}}
		{...props}
	>
		Добавить
	</Button>
);

export interface IconButtonProps extends ButtonProps {
	icon: React.ReactNode,
}

export const StartIconButton = (props: IconButtonProps) => (
	<Button
		variant="contained"
		loadingPosition="start"
		startIcon={props.icon}
		sx={{
			marginLeft: '1vw',
			marginRight: '1vw',
			marginTop: '2px',
			marginBottom: '2px',
			width: 'fill-available',
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			},
		}}
		{...props}
	/>
);

export const EndIconButton = (props: IconButtonProps) => (
	<Button
		variant="contained"
		loadingPosition="end"
		endIcon={props.icon}
		sx={{
			marginLeft: '1vw',
			marginRight: '1vw',
			marginTop: '2px',
			marginBottom: '2px',
			width: 'fill-available',
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			},
		}}
		{...props}
	/>
);

export const ItemsList = styled(List)({
	padding: 0
});
import styled from "@emotion/styled";
import { Box, Typography, ButtonProps, Button } from "@mui/material";
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';

export const ElementContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
	width: 'fill-available',
	marginLeft: '2vw',
	marginRight: '2vw',
});

export const HeaderLabel = styled(Typography)({
	paddingTop: '12px',
	paddingBottom: '12px',
	fontSize: '1rem',
	fontWeight: '600'
});

export const ActionsBar = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
	justifyContent: 'space-between',
	marginBottom: '1vw',
});

export const SaveButton = (props: ButtonProps) => (
	<Button
		variant="contained"
		endIcon={<SaveIcon/>}
		sx={{
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			}
		}}
		{...props}
	>
		Сохранить
	</Button>
);

export const DeleteButton = (props: ButtonProps) => (
	<Button
		variant="contained"
		startIcon={<DeleteIcon/>}
		sx={{
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			}
		}}
		{...props}
	>
		Удалить
	</Button>
);
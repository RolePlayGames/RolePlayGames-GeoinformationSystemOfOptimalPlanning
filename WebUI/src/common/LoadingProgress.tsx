import { Box, CircularProgress, styled } from "@mui/material";

const LoadingProgressContainer = styled(Box)({
	width: '100%',
	minWidth: '3vw',
	height: '100%',
	minHeight: '3vw',
	display: 'flex',
	alignItems: 'center',
	justifyContent: 'center',
	position: 'fixed',
});

const ProgressElement = styled(CircularProgress)({
	width: '6vw',
	height: '6vw',
	color: '#1d1b31',
});

export const LoadingProgress = () => (
	<LoadingProgressContainer>
		<ProgressElement/>
	</LoadingProgressContainer>
);
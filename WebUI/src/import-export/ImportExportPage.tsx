import { Box, Stack, Button, ButtonProps, styled } from "@mui/material";
import { HeaderLabel, PageContainer, StartIconButton } from "../common/controls";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import FileDownloadOutlinedIcon from '@mui/icons-material/FileDownloadOutlined';
import { useState } from "react";
import { LoadingProgress } from "../common/LoadingProgress";

const ImportExportContianer = styled(Box)({
	display: 'flex',
	justifyContent: 'center',
	alignItems: 'center',
	flexDirection: 'column',
	height: '100vh',
});

const IconButton = styled(Button)({
	marginLeft: '1vw',
	marginRight: '1vw',
	marginTop: '2px',
	marginBottom: '2px',
	width: 'fill-available',
	background: '#1d1b31',
	'&:hover': {
		backgroundColor: '#11101d'
	},
})

const ImportButton = (props: ButtonProps) => (
	<StartIconButton
		text='Импорт'
		icon={<FileUploadOutlinedIcon/>}
		{...props}
	/>
);

const ExportButton = (props: ButtonProps) => (
	<StartIconButton
		text='Экспорт'
		icon={<FileDownloadOutlinedIcon/>}
		{...props}
	/>
);

export const ImportExportPage = () => {
	const [isActionsDisabled, SetIsActionDisabled] = useState(false);

	const onImport = () => {
		SetIsActionDisabled(true);
	}
    
	const onExport = () => {
		SetIsActionDisabled(true);
	}

	return (
		<PageContainer>
			<HeaderLabel>Импорт / Экспорт</HeaderLabel>
			<ImportExportContianer>
				{ isActionsDisabled ? (
					<LoadingProgress/>
				) : (
					<Stack direction="column" spacing={5}>
						<ImportButton disabled={isActionsDisabled} onClick={onImport}/>
						<ExportButton disabled={isActionsDisabled} onClick={onExport}/>
					</Stack>
				)}
			</ImportExportContianer>
		</PageContainer>
	)}

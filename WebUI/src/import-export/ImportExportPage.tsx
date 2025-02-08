import { Box, Stack, ButtonProps, styled } from "@mui/material";
import { HeaderLabel, PageContainer, StartIconButton } from "../common/controls";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import FileDownloadOutlinedIcon from '@mui/icons-material/FileDownloadOutlined';
import { useState } from "react";

const ImportExportContianer = styled(Box)({
	display: 'flex',
	justifyContent: 'center',
	alignItems: 'center',
	flexDirection: 'column',
	height: '100vh',
});

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
				<Stack direction="column" spacing={5}>
					<ImportButton loading={isActionsDisabled} onClick={onImport}/>
					<ExportButton loading={isActionsDisabled} onClick={onExport}/>
				</Stack>
			</ImportExportContianer>
		</PageContainer>
	)}

import { Box, Stack, ButtonProps, styled } from "@mui/material";
import { HeaderLabel, PageContainer, StartIconButton } from "../common/controls";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import FileDownloadOutlinedIcon from '@mui/icons-material/FileDownloadOutlined';
import { useCallback, useEffect, useState } from "react";
import { exportProductionData, importProductionData } from "./importExportClient";
import { LoadingProgress } from "../common/LoadingProgress";

const ImportExportContianer = styled(Box)({
	display: 'flex',
	justifyContent: 'center',
	alignItems: 'center',
	flexDirection: 'column',
	height: '100vh',
});

const VisuallyHiddenInput = styled('input')({
	clip: 'rect(0 0 0 0)',
	clipPath: 'inset(50%)',
	height: 1,
	overflow: 'hidden',
	position: 'absolute',
	bottom: 0,
	left: 0,
	whiteSpace: 'nowrap',
	width: 1,
});

interface ImportButtonProps extends ButtonProps {
	uploadFiles: (files: FileList | null) => void,
}

const ImportButton = (props: ImportButtonProps) => (
	<StartIconButton
		component="label"
		icon={<FileUploadOutlinedIcon />}
	>
		Импорт
		<VisuallyHiddenInput
			type='file'
			onChange={(event) => props.uploadFiles(event.target.files)}
			accept='.xlsx'
		/>
	</StartIconButton>
);

const ExportButton = (props: ButtonProps) => (
	<StartIconButton
		icon={<FileDownloadOutlinedIcon/>}
		{...props}
	>
		Экспорт
	</StartIconButton>
);

const generateExportFileName = () => {
	const now = new Date();
	const formattedDateTime = now.toISOString().replace(/[-:T.]/g, '_').slice(0, 14);
	return `ProductionData_${formattedDateTime}.xlsx`;
}

export const ImportExportPage = () => {
	const [isActionsDisabled, SetIsActionDisabled] = useState(false);
	const [formData, SetFormData] = useState<FormData>();

	const uploadFile = useCallback(async (formData: FormData) => {
		try {
			await importProductionData(formData);
		} catch(error: any) {
			console.log(`Got an error while uploading file: ${error}`);
		} finally {
			SetIsActionDisabled(false);
		}
	}, [formData]);

	useEffect(() => {
		if (formData)
			uploadFile(formData);
	}, [formData]);

	const onImport = (files: FileList | null) => {
		if (!files)
			return;

		const file = files[0];

		if (!file)
			return;

		const formData = new FormData();
		formData.append('file', file);

		SetIsActionDisabled(true);
		SetFormData(formData);
	}
    
	const onExport = async () => {
		SetIsActionDisabled(true);

		try {
			const data = await exportProductionData();
			const url = window.URL.createObjectURL(data);

			const link = document.createElement('a');
			link.href = url;
			link.setAttribute('download', generateExportFileName());

			document.body.appendChild(link);
			link.click();
			document.body.removeChild(link);
		} catch(error: any) {
			console.log(`Got an error while exporting file: ${error}`);
		} finally {
			SetIsActionDisabled(false);
		}
	}

	return (
		<PageContainer>
			<HeaderLabel>Импорт / Экспорт</HeaderLabel>
			<ImportExportContianer>
				{ isActionsDisabled ? (
					<LoadingProgress/>
				) : (
					<Stack direction="column" spacing={5}>
						<ImportButton loading={isActionsDisabled} uploadFiles={onImport}/>
						<ExportButton loading={isActionsDisabled} onClick={onExport}/>
					</Stack>
				)}
			</ImportExportContianer>
		</PageContainer>
	)
}

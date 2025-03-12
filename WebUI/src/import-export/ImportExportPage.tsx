import { Box, Stack, ButtonProps, styled } from "@mui/material";
import { HeaderLabel, PageContainer, StartIconButton } from "../common/controls";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import FileDownloadOutlinedIcon from '@mui/icons-material/FileDownloadOutlined';
import { useCallback, useEffect, useState } from "react";
import { exportProductionData, importProductionData, ProductionDataImportItemNotFoundError } from "./importExportClient";
import { LoadingProgress } from "../common/LoadingProgress";
import { toast } from "react-toastify";
import { ClientError } from "../common/clients/clientError";

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
			const importResult = await importProductionData(formData);
			toast.success(`Данные успешно загружены. Всего заказов: ${importResult.ordersCount}, производственных линий: ${importResult.productionLinesCount}`);
		} catch (error: unknown) {
			const productionDataImportItemNotFoundError = error as ProductionDataImportItemNotFoundError;

			if (productionDataImportItemNotFoundError.errorType === 'ProductionDataImportItemNotFoundError') 
				if (productionDataImportItemNotFoundError.itemType === 'FilmTypeDTO')
					toast.error(`Найдена ссылка на несуществующий тип пленки ${productionDataImportItemNotFoundError.identifier}`);
				else if (productionDataImportItemNotFoundError.itemType === 'CustomerDTO')
					toast.error(`Найдена ссылка на несуществующего заказчика ${productionDataImportItemNotFoundError.identifier}`);
				else if (productionDataImportItemNotFoundError.itemType === 'FilmRecipeDTO')
					toast.error(`Найдена ссылка на несуществующий рецепт ${productionDataImportItemNotFoundError.identifier}`);
				else
					toast.error(`Найдена ссылка на несуществующий элемент ${productionDataImportItemNotFoundError.identifier}`);
			
			else if (error instanceof ClientError) {
				const errorCode = error.errorCode;
	
				if (errorCode === 'CustomerNameAlreadyExistsException')
					toast.error('Найдены дублирующие имена заказчиков');
				else if (errorCode === 'FilmRecipeNameAlreadyExistsException')
					toast.error('Найдены дублирующие названия рецептов');
				else if (errorCode === 'FilmTypeDoesNotExistsException')
					toast.error('Найдены ссылки на несуществующие типы пленки');
				else if (errorCode === 'FilmTypeArticleAlreadyExistsException')
					toast.error('Найдены дублирующие типы пленки');
				else if (errorCode === 'OrderNumberAlreadyExistsException')
					toast.error('Найдены дублирующие заказы');
				else if (errorCode === 'CustomerDoesNotExistsException')
					toast.error('Найдены ссылки на несуществующих заказчиков');
				else if (errorCode === 'FilmRecipeDoesNotExistsException')
					toast.error('Найдены ссылки на несуществующие рецепты');
				else if (errorCode === 'ProductionLineNameAlreadyExistsException')
					toast.error('Найдены дублирующие производственные линии');
				else if (errorCode === 'ProductionDataEndImportException')
					toast.error('Произошла ошибка при сохранении данных. Проверьте данные и попробуйте еще раз позже');
				else if (errorCode === 'ArgumentOutOfRangeException')
					toast.error('Найдены некорректные данные. Проверьте данные и попробуйте еще раз');
			}
			else
				toast.error('Произошла ошибка при загрузке данных. Проверьте корректность данных');
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
		} catch (error: any) {
			toast.error('Произошла ошибка при выгрузке данных');
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

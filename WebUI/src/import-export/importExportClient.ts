import axios, { AxiosError } from "axios";
import { ClientError } from "../common/clients/clientError";
import { handleError } from "../common/clients/clients";
import { nameof } from "../utils/nameof/nameof";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'production-data';

export interface ProductionDataImportItemNotFoundError {
	errorType: string;
	itemType: string;
	identifier: string;
}

export type ImportProductionDataResult = {
	ordersCount: number,
	productionLinesCount: number,
}

export const importProductionData = async (file: FormData) => {
	try {
		const { data } = await axios.postForm<ImportProductionDataResult>(`${API_ROOT}${API_URL}/excel/import`, file);
		return data;
	} catch (error: unknown) {
		const axiosError = error as AxiosError;

		handleError(nameof({importProductionData}), axiosError);

		if (axiosError?.response?.status === 422 && (error as ProductionDataImportItemNotFoundError)?.errorType === 'ProductionDataImportItemNotFoundError')
			throw error;

		throw new ClientError(axiosError);
	}
}

export const exportProductionData = async () => {
	const response = await axios.get(`${API_ROOT}${API_URL}/excel/export`, {
		responseType: 'blob', // response type for binary data
	});

	return new Blob([response.data]);
}
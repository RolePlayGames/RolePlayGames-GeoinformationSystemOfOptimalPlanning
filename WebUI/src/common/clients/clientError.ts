import { AxiosError } from "axios";

export interface IClientError {
    status: number;
	errorCode: string;
}

export class ClientError extends Error implements IClientError {
	status: number;
	errorCode: string;
	constructor(error: AxiosError) {
		super(error.message);

		this.status = error.response?.status as number;
		this.errorCode = error.response?.data as string;
	}
}
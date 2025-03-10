import { AxiosError } from "axios";

export const handleError = (methodName: string, error: AxiosError | undefined) => {
	if (error) 
		console.log(`${methodName} получил ответ ${error.response?.status} с сообщением ${error.response?.data} and error: ${error}`);   
}
import axios from "axios";

const API_ROOT = `${document.location.protocol}//${document.location.host}/api/`;
const API_URL = 'production-data';

export const importProductionData = async (file: FormData) => {
	await axios.postForm(`${API_ROOT}${API_URL}/excel/import`, file);
}
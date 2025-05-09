import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";
import { FilmType, getFilmType } from "./filmTypesClient";
import { FilmTypeElement } from "./FilmTypeElement";
import { toast } from "react-toastify";

type FilmTypePageProps = {
    id: number,
    apiPath: string,
}

export const FilmTypePage = ({ id, apiPath }: FilmTypePageProps) => {

	const [item, setItem] = useState<FilmType | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: FilmType | undefined;
				
		try {
			if (id > 0) 
				item = await getFilmType(id);
			else 
				item = {
					article: '',
				};
		} catch {
			toast.error('Произошла ошибка при загрузке данных');
		}
        
		if (item)
			setItem(item);
		else
			navigate(apiPath);
	}, [id]);
    
	useEffect(() => {
		loadItem();
	}, [id]);

	if (item === undefined) 
		return (
			<LoadingProgress/>
		);
	else 
		return (
			<FilmTypeElement id={id} item={item} apiPath={apiPath}/>
		);
    
}
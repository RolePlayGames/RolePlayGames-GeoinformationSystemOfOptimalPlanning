import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";
import { AvaliableFilmType, FilmRecipe, getFilmRecipe } from "./filmRecipesClient";
import { FilmRecipeElement } from "./FilmRecipeElement";
import { toast } from "react-toastify";

type FilmRecipePageProps = {
    id: number,
    apiPath: string,
	filmTypes: AvaliableFilmType[],
}

export const FilmRecipePage = ({ id, apiPath, filmTypes }: FilmRecipePageProps) => {

	const [item, setItem] = useState<FilmRecipe | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: FilmRecipe | undefined;
		
		try {
			if (id > 0) 
				item = await getFilmRecipe(id);
			else 
				item = {
					name: '',
					filmTypeID: 0,
					thickness: 0,
					productionSpeed: 0,
					materialCost: 0,
					nozzle: 0,
					calibration: 0,
					coolingLip: 0,
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
			<FilmRecipeElement id={id} item={item} apiPath={apiPath} filmTypes={filmTypes}/>
		);
    
}
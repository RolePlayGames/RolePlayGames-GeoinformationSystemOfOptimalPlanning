import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { LoadingProgress } from "../common/LoadingProgress";
import { ProductionElement } from "./ProductionElement";
import { Production, getProduction } from "./productionsClient";

type ProductionPageProps = {
    id: number,
    apiPath: string,
}

export const ProductionPage = ({ id, apiPath }: ProductionPageProps) => {

	const [item, setItem] = useState<Production | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: Production | undefined;

		try {
			if (id > 0) 
				item = await getProduction(id);
			else 
				item = {
					name: '',
					coordinates: undefined,
				}
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
			<ProductionElement id={id} item={item} apiPath={apiPath}/>
		);
    
}
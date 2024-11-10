import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";
import { ProductionLine, getProductionLine } from "./productionLinesClient";
import { ProductionLineElement } from "./ProductionLineElement";

type ProductionLinePageProps = {
    id: number,
    apiPath: string,
}

export const ProductionLinePage = ({ id, apiPath }: ProductionLinePageProps) => {

	const [item, setItem] = useState<ProductionLine | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: ProductionLine | undefined;

		if (id > 0) 
			item = await getProductionLine(id);
		else 
			item = {
				name: '',
				hourCost: 0,
				maxProductionSpeed: 0,
				widthMin: 0,
				widthMax: 0,
				thicknessMin: 0,
				thicknessMax: 0,
				thicknessChangeTime: '00:00:00',
				thicknessChangeConsumption: 0,
				widthChangeTime: '00:00:00',
				widthChangeConsumption: 0,
				setupTime: '00:00:00',
			};        
        
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
			<ProductionLineElement id={id} item={item} apiPath={apiPath}/>
		);
    
}
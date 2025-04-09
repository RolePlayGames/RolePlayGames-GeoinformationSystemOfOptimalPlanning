import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { LoadingProgress } from "../common/LoadingProgress";
import { getRoute, RouteReadModel } from "./routesClient";
import { RouteElement } from "./RouteElement";

type RoutePageProps = {
    id: number,
    apiPath: string,
}

export const RoutePage = ({ id, apiPath }: RoutePageProps) => {

	const [item, setItem] = useState<RouteReadModel | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: RouteReadModel | undefined;

		try {
			item = await getRoute(id);

			if (item !== undefined) {
				if (!item.productionInfo.entityCoordinates)
					toast.error(`Производство ${item.productionInfo.entityName} не имеет данных о местоположении. Карта будет доступна после того, как заполните данные`);
                
				if (!item.customerInfo.entityCoordinates)
					toast.error(`Заказчик ${item.customerInfo.entityName} не имеет данных о местоположении. Карта будет доступна после того, как заполните данные`);
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
			<RouteElement id={id} item={item} apiPath={apiPath}/>
		);
}
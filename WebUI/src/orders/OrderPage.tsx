import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";
import { Order, getOrder } from "./ordersClient";
import { OrderElement } from "./OrderElement";
import { CustomerInfo } from "../customers/customersClient";
import { FilmRecipeInfo } from "../film-recipes/filmRecipesClient";
import { toast } from "react-toastify";

type OrderPageProps = {
    id: number,
    apiPath: string,
	customers: CustomerInfo[],
	filmRecipes: FilmRecipeInfo[],
}

export const OrderPage = ({ id, apiPath, customers, filmRecipes }: OrderPageProps) => {

	const [item, setItem] = useState<Order | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: Order | undefined;
				
		try {
			if (id > 0) 
				item = await getOrder(id);
			else 
				item = {
					number: '',
					customerID: 0,
					filmRecipeID: 0,
					width: 0,
					quantityInRunningMeter: 0,
					finishedGoods: 0,
					waste: 0,
					rollsCount: 0,
					plannedDate: new Date(),
					priceOverdue: 0,
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
			<OrderElement id={id} item={item} apiPath={apiPath} customers={customers} filmRecipes={filmRecipes}/>
		);
    
}
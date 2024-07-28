import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { LoadingProgress } from "../common/LoadingProgress";
import { AddItemButton, HeaderLabel, ItemsBlock, ItemsContainer, ItemsList, PageContainer } from "../common/controls";
import { Item, ListItem } from "../common/Item";
import { OrderPage } from "./OrderPage";
import { getOrdersInfo } from "./ordersClient";
import { ORDERS } from "../routes/routes";
import { CustomerInfo, getCustomersInfo } from "../customers/customersClient";
import { FilmRecipeInfo, getFilmRecipesInfo } from "../film-recipes/filmRecipesClient";

const apiPath = ORDERS;

export const OrdersPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();
	const [customers, setCustomers] = useState<CustomerInfo[]>();
	const [filmRecipes, setFilmRecipes] = useState<FilmRecipeInfo[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		const items = await getOrdersInfo();      
		setItems(items);

		const customers = await getCustomersInfo();
		setCustomers(customers);

		const filmRecipes = await getFilmRecipesInfo();
		setFilmRecipes(filmRecipes);
	}, []);

	useEffect(() => {
		const numberId = id === 'new' ? 0 : Number(id);
		setItemId(numberId);

		const vis = id !== undefined;
		setIsVisible(vis);

		if (!id)
			loadItems();
	}, [id]);
    
	const onAdd = () => {
		navigate(`${apiPath}/new`);
	}

	const handleItemClick = (item: ListItem) => {
		navigate(`${apiPath}/${item.id}`);
	}

	return(
		<PageContainer>
			<HeaderLabel>Заказы</HeaderLabel>
			{ items === undefined || customers === undefined || customers.length === 0 || filmRecipes === undefined || filmRecipes.length === 0 ? (
				<LoadingProgress/>
			) : (
				<ItemsContainer>
					<ItemsBlock>
						<AddItemButton onClick={onAdd}/>
						<ItemsList>
							{ items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
						</ItemsList>
					</ItemsBlock>
					{ (itemId !== undefined && isVisible) && <OrderPage id={itemId} apiPath={apiPath} customers={customers} filmRecipes={filmRecipes}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
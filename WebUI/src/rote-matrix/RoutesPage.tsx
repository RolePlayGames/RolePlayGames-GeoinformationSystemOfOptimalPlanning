import { useState, useCallback, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { PageContainer, HeaderLabel, ItemsContainer, ItemsBlock, ItemsList } from "../common/controls";
import { ListItem, Item } from "../common/Item";
import { LoadingProgress } from "../common/LoadingProgress";
import { ROUTES } from "../routes/routes";
import { getRoutesInfo } from "./routesClient";
import { RoutePage } from "./RoutePage";

const apiPath = ROUTES;

export const RoutesPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		try {
			const result = await getRoutesInfo();
			setItems(result?.sort((a, b) => a.id - b.id));
		} catch {
			toast.error('Произошла ошибка при чтении данных');
		}
	}, []);

	useEffect(() => {
		const numberId = id === 'new' ? 0 : Number(id);
		setItemId(numberId);

		const vis = id !== undefined;
		setIsVisible(vis);

		if (!id)
			loadItems();
	}, [id]);

	const handleItemClick = (item: ListItem) => {
		navigate(`${apiPath}/${item.id}`);
	}

	return(
		<PageContainer>
			<HeaderLabel>Матрица маршрутов</HeaderLabel>
			{ items === undefined ? (
				<LoadingProgress/>
			) : (
				<ItemsContainer>
					<ItemsBlock>
						<ItemsList>
							{ items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
						</ItemsList>
					</ItemsBlock>
					{ (itemId !== undefined && isVisible) && <RoutePage id={itemId} apiPath={apiPath}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
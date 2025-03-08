import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { CustomerPage } from "./CustomerPage";
import { getCustomersInfo } from "./customersClient";
import { LoadingProgress } from "../common/LoadingProgress";
import { AddItemButton, HeaderLabel, ItemsBlock, ItemsContainer, ItemsList, PageContainer } from "../common/controls";
import { Item, ListItem } from "../common/Item";
import { CUSTOMERS } from "../routes/routes";
import { toast } from "react-toastify";

const apiPath = CUSTOMERS;

export const CustomersPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		try {
			const result = await getCustomersInfo();
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
    
	const onAdd = () => {
		navigate(`${apiPath}/new`);
	}

	const handleItemClick = (item: ListItem) => {
		navigate(`${apiPath}/${item.id}`);
	}

	return(
		<PageContainer>
			<HeaderLabel>Заказчики</HeaderLabel>
			{ items === undefined ? (
				<LoadingProgress/>
			) : (
				<ItemsContainer>
					<ItemsBlock>
						<AddItemButton onClick={onAdd}/>
						<ItemsList>
							{ items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
						</ItemsList>
					</ItemsBlock>
					{ (itemId !== undefined && isVisible) && <CustomerPage id={itemId} apiPath={apiPath}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
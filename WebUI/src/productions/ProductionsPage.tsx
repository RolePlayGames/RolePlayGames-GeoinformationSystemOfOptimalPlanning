import { useState, useCallback, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { PageContainer, HeaderLabel, ItemsContainer, ItemsBlock, AddItemButton, ItemsList } from "../common/controls";
import { ListItem, Item } from "../common/Item";
import { LoadingProgress } from "../common/LoadingProgress";
import { PRODUCTIONS } from "../routes/routes";
import { ProductionPage } from "./ProductionPage";
import { getProductionsInfo } from "./productionsClient";

const apiPath = PRODUCTIONS;

export const ProductionsPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		try {
			const result = await getProductionsInfo();
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
			<HeaderLabel>Производства</HeaderLabel>
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
					{ (itemId !== undefined && isVisible) && <ProductionPage id={itemId} apiPath={apiPath}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
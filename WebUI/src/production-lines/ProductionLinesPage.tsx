import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { LoadingProgress } from "../common/LoadingProgress";
import { AddItemButton, HeaderLabel, ItemsBlock, ItemsContainer, ItemsList, PageContainer } from "../common/controls";
import { Item, ListItem } from "../common/Item";
import { ProductionLinePage } from "./ProductionLinePage";
import { getProductionLinesInfo } from "./productionLinesClient";
import { PRODUCTION_LINES } from "../routes/routes";
import { AvaliableFilmType, getAvaliableFilmTypes } from "../film-recipes/filmRecipesClient";
import { toast } from "react-toastify";
import { getProductionsInfo, ProductionInfo } from "../productions/productionsClient";

const apiPath = PRODUCTION_LINES;

export const ProductionLinesPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();
	const [filmTypes, setFilmTypes] = useState<AvaliableFilmType[]>();
	const [productions, setProductions] = useState<ProductionInfo[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		try {
			const items = await getProductionLinesInfo();      
			setItems(items?.sort((a, b) => a.id - b.id));
			
			const filmTypes = await getAvaliableFilmTypes();
			setFilmTypes(filmTypes);
				
			const productions = await getProductionsInfo();
			setProductions(productions);

			if (filmTypes?.length === 0)
				toast.error('В системе отсутствуют типы материала. Для создания правил производственных линий создайте хотя бы одно производство');

			if (productions?.length === 0)
				toast.error('В системе отсутствуют производства. Для создания производственных линий создайте хотя бы одно производство');
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
			<HeaderLabel>Производственные линии</HeaderLabel>
			{ items === undefined || filmTypes === undefined || productions === undefined ? (
				<LoadingProgress/>
			) : (
				<ItemsContainer>
					<ItemsBlock>
						<AddItemButton onClick={onAdd}/>
						<ItemsList>
							{ items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
						</ItemsList>
					</ItemsBlock>
					{ (itemId !== undefined && isVisible) && <ProductionLinePage id={itemId} apiPath={apiPath} filmTypes={filmTypes} productions={productions}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
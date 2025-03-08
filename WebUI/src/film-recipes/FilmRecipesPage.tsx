import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { LoadingProgress } from "../common/LoadingProgress";
import { AddItemButton, HeaderLabel, ItemsBlock, ItemsContainer, ItemsList, PageContainer } from "../common/controls";
import { Item, ListItem } from "../common/Item";
import { FilmRecipePage } from "./FilmRecipePage";
import { AvaliableFilmType, getAvaliableFilmTypes, getFilmRecipesInfo } from "./filmRecipesClient";
import { FILM_RECIPES } from "../routes/routes";
import { toast } from "react-toastify";

const apiPath = FILM_RECIPES;

export const FilmRecipesPage = () => {

	const { id } = useParams();

	const [itemId, setItemId] = useState<number | undefined>();
	const [isVisible, setIsVisible] = useState(false);

	const [items, setItems] = useState<ListItem[]>();
	const [filmTypes, setFilmTypes] = useState<AvaliableFilmType[]>();

	const navigate = useNavigate();

	const loadItems = useCallback(async () => {
		try {
			const items = await getFilmRecipesInfo();
			setItems(items);
		
			const filmTypes = await getAvaliableFilmTypes();
			setFilmTypes(filmTypes);

			if (filmTypes?.length === 0)
				toast.error('В системе отсутствуют типы пленки. Для создания рецептов создайте хотя бы один тип пленки');
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
			<HeaderLabel>Рецепты пленок</HeaderLabel>
			{ items === undefined || filmTypes === undefined || filmTypes.length === 0 ? (
				<LoadingProgress/>
			) : (
				<ItemsContainer>
					<ItemsBlock>
						<AddItemButton onClick={onAdd}/>
						<ItemsList>
							{ items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
						</ItemsList>
					</ItemsBlock>
					{ (itemId !== undefined && isVisible) && <FilmRecipePage id={itemId} apiPath={apiPath} filmTypes={filmTypes}/> }
				</ItemsContainer>
			)}
		</PageContainer>
	);
}
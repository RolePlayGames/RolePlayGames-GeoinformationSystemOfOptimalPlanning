import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { InputField } from "../common/inputs";
import { FilmType, createFilmType, deleteFilmType, updateFilmType } from "./filmTypesClient";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { useItemFieldWithValidation } from "../common/useItemField";
import { IClientError } from "../common/clients/clientError";
import { toast } from "react-toastify";

const validateArticle = (article: string) => {
	if (article.length == 0)
		return 'Заполните название';

	if (article.length > 10)
		return 'Название не должно превышать 10 символов';

	return undefined;
}

type FilmTypeElementProps = {
    id: number,
    item: FilmType,
    apiPath: string,
}

export const FilmTypeElement = ({ id, item, apiPath }: FilmTypeElementProps) => {
	const [article, setArticle, articleError, setArticleError] = useItemFieldWithValidation<FilmType, string>(item, x => x.article, validateArticle);

	const navigate = useNavigate();
    
	useEffect(() => {
		setArticleError(validateArticle(article));
	}, [article]);

	const onUpdate = async () => {
		try {
			await updateFilmType(id, { article });
			navigate(apiPath);
			toast.success(`Тип ${article} был обновлен`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmTypeArticleAlreadyExistsException') {
				toast.error(`Произошла ошибка при обновлении: указанное название уже используется в другом типом`);
				setArticleError('Указанное название уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);
			
		}
	};

	const onCreate = async () => {
		try {
			await createFilmType({ article });
			navigate(apiPath);
			toast.success(`Тип ${article} был создан`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmTypeArticleAlreadyExistsException') {
				toast.error(`Произошла ошибка при создании: указанное название уже используется в другим типом`);
				setArticleError('Указанное название уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при создании. Проверьте параметры`);
			
		}
	};

	const onSave = () => {
		if (id > 0) 
			return onUpdate();
		else 
			return onCreate();
        
	};

	const onDelete = async () => {
		if (id > 0) {
			await deleteFilmType(id);
			navigate(apiPath);
		}
	};

	return(
		<ElementContainer>
			<HeaderLabel>Тип пленки {item.article}</HeaderLabel>
			<ActionsBar>
				<SaveButton onClick={onSave} disabled={!!articleError}/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<InputField
				label='Название'
				value={article}
				onChange={setArticle}
				errorText={articleError}/>
		</ElementContainer>
	);
}
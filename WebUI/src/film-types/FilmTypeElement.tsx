import { Box, Typography, Button, styled, ButtonProps } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CommonInputField } from "../common/inputs/inputs";
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';
import { FilmType, IClientError, createFilmType, deleteFilmType, updateFilmType } from "./filmTypesClient";

const FilmTypeElementContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
	width: 'fill-available',
	marginLeft: '2vw',
	marginRight: '2vw',
});

const HeaderLabel = styled(Typography)({
	paddingTop: '12px',
	paddingBottom: '12px',
	fontSize: '1rem',
	fontWeight: '600'
});

const ActionsBar = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
	justifyContent: 'space-between',
	marginBottom: '1vw',
});

const SaveButton = (props: ButtonProps) => (
	<Button
		variant="contained"
		endIcon={<SaveIcon/>}
		sx={{
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			}
		}}
		{...props}
	>
		Сохранить
	</Button>
);

const DeleteButton = (props: ButtonProps) => (
	<Button
		variant="contained"
		startIcon={<DeleteIcon/>}
		sx={{
			background: '#1d1b31',
			'&:hover': {
				backgroundColor: '#11101d'
			}
		}}
		{...props}
	>
		Удалить
	</Button>
);

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

export const FilmTypeElement = ({ id, item, apiPath }: FilmTypeElementProps)=> {
	const [article, setArticle] = useState(item.article);
	const [articleError, setArticleError] = useState<string>();

	const navigate = useNavigate();
    
	useEffect(() => {
		setArticleError(validateArticle(article));
	}, [article]);

	const onUpdate = async () => {
		try {
			await updateFilmType(id, { article });
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmTypeArticleAlreadyExistsException')
				setArticleError('Указанное название уже используется в системе');
		}
	};

	const onCreate = async () => {
		try {
			await createFilmType({ article });
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'FilmTypeArticleAlreadyExistsException')
				setArticleError('Указанное название уже используется в системе');
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

	const changeArticle = (event: React.ChangeEvent<HTMLInputElement>) => {
		setArticle(event.target.value);
	};

	return(
		<FilmTypeElementContainer>
			<HeaderLabel>Тип пленки {item.article}</HeaderLabel>
			<ActionsBar>
				<SaveButton onClick={onSave} disabled={!!articleError}/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<CommonInputField
				label={'Название'}
				value={article}
				onChange={changeArticle}
				error={!!articleError}
				helperText={articleError}
				sx={{
					marginTop: '1vw',
					marginBottom: '1vw',
				}}/>
		</FilmTypeElementContainer>
	);
}
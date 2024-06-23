import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CommonInputField } from "../common/inputs/inputs";
import { Customer, updateCustomer, createCustomer, deleteCustomer, IClientError } from "./customersClient";
import { HeaderLabel } from "../common/controls";
import { ActionsBar, SaveButton, DeleteButton, ElementContainer } from "../common/elementControls";

const validateName = (name: string) => {
	if (name.length == 0)
		return 'Заполните название';

	if (name.length > 500)
		return 'Название не должно превышать 500 символов';

	return undefined;
}

type CustomerElementProps = {
    id: number,
    item: Customer,
    apiPath: string,
}

export const CustomerElement = ({ id, item, apiPath }: CustomerElementProps)=> {
	const [name, setName] = useState(item.name);
	const [nameError, setNameError] = useState<string>();

	const navigate = useNavigate();
    
	useEffect(() => {
		setNameError(validateName(name));
	}, [name]);

	const changeName = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const onUpdate = async () => {
		try {
			await updateCustomer(id, { name });
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException')
				setNameError('Указанное имя уже используется в системе');
		}
	};

	const onCreate = async () => {
		try {
			await createCustomer({ name });
			navigate(apiPath);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException')
				setNameError('Указанное имя уже используется в системе');
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
			await deleteCustomer(id);
			navigate(apiPath);
		}
	};

	return(
		<ElementContainer>
			<HeaderLabel>Заказчик {item.name}</HeaderLabel>
			<ActionsBar>
				<SaveButton onClick={onSave} disabled={!!nameError}/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<CommonInputField
				label={'Название'}
				value={name}
				onChange={changeName}
				error={!!nameError}
				helperText={nameError}
				sx={{
					marginTop: '1vw',
					marginBottom: '1vw',
				}}/>
		</ElementContainer>
	);
}
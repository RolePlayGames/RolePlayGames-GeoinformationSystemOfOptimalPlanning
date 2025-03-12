import { useNavigate } from "react-router-dom";
import { InputField } from "../common/inputs";
import { Customer, updateCustomer, createCustomer, deleteCustomer } from "./customersClient";
import { HeaderLabel } from "../common/controls";
import { ActionsBar, SaveButton, DeleteButton, ElementContainer } from "../common/elementControls";
import { useItemFieldWithValidation } from "../common/useItemField";
import { IClientError } from "../common/clients/clientError";
import { toast } from "react-toastify";

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
	const [name, setName, nameError, setNameError] = useItemFieldWithValidation<Customer, string>(item, x => x.name, validateName);

	const navigate = useNavigate();

	const onUpdate = async () => {
		try {
			await updateCustomer(id, { name });
			navigate(apiPath);
			toast.success(`Заказчик ${name} был обновлен`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при обновлении: указанное имя уже используется другим заказчиком`);
				setNameError('Указанное имя уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);
			
		}
	};

	const onCreate = async () => {
		try {
			await createCustomer({ name });
			navigate(apiPath);
			toast.success(`Заказчик ${name} был создан`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при создании: указанное имя уже используется другим заказчиком`);
				setNameError('Указанное имя уже используется в системе');
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
			<InputField
				label='Название'
				value={name}
				onChange={setName}
				errorText={nameError}/>
		</ElementContainer>
	);
}
import { useNavigate } from "react-router-dom";
import { InputField } from "../common/inputs";
import { Customer, updateCustomer, createCustomer, deleteCustomer } from "./customersClient";
import { HeaderLabel } from "../common/controls";
import { ActionsBar, SaveButton, DeleteButton, ElementContainer } from "../common/elementControls";
import { useItemField, useItemFieldWithValidation } from "../common/useItemField";
import { IClientError } from "../common/clients/clientError";
import { toast } from "react-toastify";
import { LocationPicker } from "../common/inputs/LocationPicker";

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
	const [latitude, setLatitude] = useItemField<Customer, number | undefined>(item, x => x.coordinates?.latitude);
	const [longitude, setLongitude] = useItemField<Customer, number | undefined>(item, x => x.coordinates?.longitude);

	const navigate = useNavigate();

	const onUpdate = async () => {
		try {
			const coordinates = latitude && longitude ? { latitude, longitude } : undefined;
			await updateCustomer(id, { name, coordinates });
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
			const coordinates = latitude && longitude ? { latitude, longitude } : undefined;
			await createCustomer({ name, coordinates });
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

	const handleLocationChange = (latitude: number | undefined, longitude: number | undefined) => {
		setLatitude(latitude);
		setLongitude(longitude);
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
			<LocationPicker initialLatitude={latitude} initialLongitude={longitude} onLocationChange={handleLocationChange}/>
		</ElementContainer>
	);
}
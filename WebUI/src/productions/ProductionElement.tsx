import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { IClientError } from "../common/clients/clientError";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { InputField } from "../common/inputs";
import { useItemFieldWithValidation, useItemField } from "../common/useItemField";
import { Production, updateProduction, createProduction, deleteProduction } from "./productionsClient";
import { LocationPicker } from "../common/inputs/LocationPicker";

const validateName = (name: string) => {
	if (name.length == 0)
		return 'Заполните название';

	if (name.length > 500)
		return 'Название не должно превышать 500 символов';

	return undefined;
}

type ProductionElementProps = {
    id: number,
    item: Production,
    apiPath: string,
}

export const ProductionElement = ({ id, item, apiPath }: ProductionElementProps)=> {
	const [name, setName, nameError, setNameError] = useItemFieldWithValidation<Production, string>(item, x => x.name, validateName);
	const [latitude, setLatitude] = useItemField<Production, number | undefined>(item, x => x.coordinates?.latitude);
	const [longitude, setLongitude] = useItemField<Production, number | undefined>(item, x => x.coordinates?.longitude);

	const navigate = useNavigate();

	const onUpdate = async () => {
		try {
			const coordinates = latitude && longitude ? { latitude, longitude } : undefined;
			await updateProduction(id, { name, coordinates });
			navigate(apiPath);
			toast.success(`Производство ${name} было обновлен`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при обновлении: указанное название уже используется другим производством`);
				setNameError('Указанное название уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);
            
		}
	};

	const onCreate = async () => {
		try {
			const coordinates = latitude && longitude ? { latitude, longitude } : undefined;
			await createProduction({ name, coordinates });
			navigate(apiPath);
			toast.success(`Производство ${name} было создан`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'ProductionNameAlreadyExistsException') {
				toast.error(`Произошла ошибка при создании: указанное название уже используется другим производством`);
				setNameError('Указанное название уже используется в системе');
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
			await deleteProduction(id);
			navigate(apiPath);
		}
	};

	const handleLocationChange = (latitude: number | undefined, longitude: number | undefined) => {
		setLatitude(latitude);
		setLongitude(longitude);
	};

	return(
		<ElementContainer>
			<HeaderLabel>Производство {item.name}</HeaderLabel>
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
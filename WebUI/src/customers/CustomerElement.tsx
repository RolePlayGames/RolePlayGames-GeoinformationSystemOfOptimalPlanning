import { Box, Typography, Button, styled, ButtonProps } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CommonInputField } from "../common/inputs/inputs";
import { Customer, updateCustomer, createCustomer, deleteCustomer, IClientError } from "./customersClient";
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';

const CustomerElementContainer = styled(Box)({
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

const validateName = (name: string) => {
    if (name.length == 0)
        return 'Заполните название';

    if (name.length > 500)
        return 'Название не должно превышать 500 символов';

    return undefined;
}

type CustomerElementProps = {
    id: number,
    customer: Customer,
}

export const CustomerElement = ({ id, customer }: CustomerElementProps)=> {

    const [name, setName] = useState(customer.name);
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
            navigate(`/customers`);
        } catch (error: unknown) {
            if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException')
                setNameError('Указанное имя уже используется в системе');
        }
    };

    const onCreate = async () => {
        try {
            await createCustomer({ name });
            navigate(`/customers`);
        } catch (error: unknown) {
            if ((error as IClientError).errorCode === 'CustomerNameAlreadyExistsException')
                setNameError('Указанное имя уже используется в системе');
        }
    };

    const onSave = () => {
        if (id > 0) {
            return onUpdate();
        } else {
            return onCreate();
        }
    };

    const onDelete = async () => {
        if (id > 0) {
            await deleteCustomer(id);
            navigate(`/customers`);
        }
    };

    return(
        <CustomerElementContainer>
            <HeaderLabel>Заказчик {customer.name}</HeaderLabel>
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
        </CustomerElementContainer>
    );
}
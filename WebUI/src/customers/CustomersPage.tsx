import { Box, Button, ButtonProps, List, ListItem, ListItemButton, ListItemText, Typography, styled } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { CustomerPage } from "./CustomerPage";
import AddCircleIcon from '@mui/icons-material/AddCircle';
import { getCustomersInfo } from "./customersClient";
import { LoadingProgress } from "../common/LoadingProgress";

const CustomersPageContainer = styled(Box)({
    display: 'flex',
    flexDirection: 'column',
});

const HeaderLabel = styled(Typography)({
    marginLeft: '1vw',
    padding: '8px',
    color: '#11101d',
    fontSize: '2rem',
    fontFamily: 'Poppins, sans-serif',
    fontWeight: '600',
});

const MenuItemButton = styled(ListItemButton)({
    minHeight: 48,
    px: 2.5,
});

const ItemsBlock = styled(Box)({
    width: 'max-content',
    borderRight: '1px',
    borderColor: 'black',
});

const AddItemButton = (props: ButtonProps) => (
    <Button
        variant="contained"
        endIcon={<AddCircleIcon/>}
        sx={{
            marginLeft: '1vw',
            marginRight: '1vw',
            marginTop: '2px',
            marginBottom: '2px',
            width: 'fill-available',
            background: '#1d1b31',
            '&:hover': {
                backgroundColor: '#11101d'
            }
        }}
        {...props}
    >
        Добавить
    </Button>
);

const ItemsList = styled(List)({
    padding: 0
});

type ItemProps = {
    item: ListItem,
    handleItemClick: (item: ListItem) => void,
}

const Item = (props: ItemProps) => (
    <ListItem key={props.item.id} sx={{ padding: 0 }}>
        <MenuItemButton onClick={() => props.handleItemClick(props.item)} sx={{ justifyContent: 'initial' }}>
            <ListItemText primary={props.item.name} sx={{ opacity: 1 }}/>
        </MenuItemButton>
    </ListItem>
);

type ListItem = {
    id: number,
    name: string,
}

export const CustomersPage = () => {

    const { id } = useParams();

    const [itemId, setItemId] = useState<number | undefined>();
    const [isVisible, setIsVisible] = useState(false);

    const [items, setItems] = useState<ListItem[]>();

    const navigate = useNavigate();

    const loadItems = useCallback(async () => {
        var result = await getCustomersInfo();      
        setItems(result);
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
        navigate(`/customers/new`);
    }

    const handleItemClick = (item: ListItem) => {
        navigate(`/customers/${item.id}`);
    }

    return(
        <CustomersPageContainer>
            <HeaderLabel>Заказчики</HeaderLabel>
            { items === undefined ? (
                <LoadingProgress/>
            ) : (
                <Box sx={{ display: 'flex', flexDirection: 'row', }}>
                    <ItemsBlock>
                        <AddItemButton onClick={onAdd}/>
                        <ItemsList>
                            { items.map((item) => (<Item item={item} handleItemClick={handleItemClick}/>)) }
                        </ItemsList>
                    </ItemsBlock>
                    { (itemId !== undefined && isVisible) && <CustomerPage id={itemId}/> }
                </Box>
            )}
        </CustomersPageContainer>
    );
}
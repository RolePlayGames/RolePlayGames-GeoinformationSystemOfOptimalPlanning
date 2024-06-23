import { ListItem, ListItemText } from "@mui/material";
import { MenuItemButton } from "./controls";

export type ListItem = {
    id: number,
    name: string,
}

type ItemProps = {
    item: ListItem,
    handleItemClick: (item: ListItem) => void,
}

export const Item = (props: ItemProps) => (
	<ListItem key={props.item.id} sx={{ padding: 0 }}>
		<MenuItemButton onClick={() => props.handleItemClick(props.item)} sx={{ justifyContent: 'initial' }}>
			<ListItemText primary={props.item.name} sx={{ opacity: 1 }}/>
		</MenuItemButton>
	</ListItem>
);
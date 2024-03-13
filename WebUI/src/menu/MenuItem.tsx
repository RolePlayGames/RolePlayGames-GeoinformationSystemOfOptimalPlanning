import { useNavigate } from "react-router-dom";
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import PeopleAltOutlinedIcon from '@mui/icons-material/PeopleAltOutlined';
import styled from "@emotion/styled";
import { white } from "../styles/colors";

export type MenuItemData = {
  header: string,
  icon: JSX.Element,
  path: string,
}
  
export const MenuItems = [
  {
    header: 'Customers',
    icon: <PeopleAltOutlinedIcon sx={{ color: white }}/>,
    path: '/customers',
  },
];

const MenuItemIcon = styled(ListItemIcon)({
    minWidth: 0,
    justifyContent: 'center',
});

const MenuItemButton = styled(ListItemButton)({
    minHeight: 48,
    px: 2.5,
});

type MenuItemProps = {
  item: MenuItemData,
  isMenuOpen: boolean,
}

export const MenuItem = ({ item, isMenuOpen }: MenuItemProps) => {
  const navigate = useNavigate();

  const handleMenuItemClick = (item: MenuItemData) => {
    navigate(item.path);
  }

  return (
    <ListItem key={item.header} disablePadding sx={{ display: 'block' }}>
      <MenuItemButton onClick={() => handleMenuItemClick(item)} sx={{ justifyContent: isMenuOpen ? 'initial' : 'center' }}>
        <MenuItemIcon sx={{ mr: isMenuOpen ? 3 : 'auto' }}>{ item.icon }</MenuItemIcon>
        <ListItemText primary={item.header} sx={{ opacity: isMenuOpen ? 1 : 0 }}/>
      </MenuItemButton>
    </ListItem>
  );
}
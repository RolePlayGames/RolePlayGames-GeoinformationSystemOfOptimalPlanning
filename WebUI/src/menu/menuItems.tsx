import PeopleAltOutlinedIcon from '@mui/icons-material/PeopleAltOutlined';
import DescriptionOutlinedIcon from '@mui/icons-material/DescriptionOutlined';
import ReceiptLongOutlinedIcon from '@mui/icons-material/ReceiptLongOutlined';
import ListAltOutlinedIcon from '@mui/icons-material/ListAltOutlined';
import PrecisionManufacturingOutlinedIcon from '@mui/icons-material/PrecisionManufacturingOutlined';
import { white } from "../styles/colors";
import { CUSTOMERS, FILM_RECIPES, FILM_TYPES, ORDERS, PRODUCTION_LINES } from "../routes/routes";

export const menuItems = [
	{
		header: 'Заказчики',
		icon: <PeopleAltOutlinedIcon sx={{ color: white }}/>,
		path: CUSTOMERS,
	},
	{
		header: 'Типы пленки',
		icon: <DescriptionOutlinedIcon sx={{ color: white }}/>,
		path: FILM_TYPES,
	},
	{
		header: 'Рецепты пленки',
		icon: <ReceiptLongOutlinedIcon sx={{ color: white }}/>,
		path: FILM_RECIPES,
	},
	{
		header: 'Заказы',
		icon: <ListAltOutlinedIcon sx={{ color: white }}/>,
		path: ORDERS,
	},
	{
		header: 'Производственные линии',
		icon: <PrecisionManufacturingOutlinedIcon sx={{ color: white }}/>,
		path: PRODUCTION_LINES,
	},
];
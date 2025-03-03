import PeopleAltOutlinedIcon from '@mui/icons-material/PeopleAltOutlined';
import DescriptionOutlinedIcon from '@mui/icons-material/DescriptionOutlined';
import ReceiptLongOutlinedIcon from '@mui/icons-material/ReceiptLongOutlined';
import ListAltOutlinedIcon from '@mui/icons-material/ListAltOutlined';
import PrecisionManufacturingOutlinedIcon from '@mui/icons-material/PrecisionManufacturingOutlined';
import ImportExportOutlinedIcon from '@mui/icons-material/ImportExportOutlined';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import { white } from "../styles/colors";
import { CUSTOMERS, FILM_RECIPES, FILM_TYPES, IMPORT_EXPORT, OPTIMAL_PLANNING, ORDERS, PRODUCTION_LINES } from "../routes/routes";

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
	{
		header: 'Импорт / Экспорт',
		icon: <ImportExportOutlinedIcon sx={{ color: white }}/>,
		path: IMPORT_EXPORT,
	},
	{
		header: 'Оптимальное планирование',
		icon: <CalendarMonthOutlinedIcon sx={{ color: white }}/>,
		path: OPTIMAL_PLANNING,
	},
];
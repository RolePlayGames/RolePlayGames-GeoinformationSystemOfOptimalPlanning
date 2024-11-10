import { SideMenu } from './menu/SideMenu';
import { Box, styled } from '@mui/material';
import { Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import { CUSTOMERS, FILM_RECIPES, FILM_TYPES, ORDERS, PRODUCTION_LINES } from './routes/routes';
import { CustomersPage } from './customers/CustomersPage';
import { FilmTypesPage } from './film-types/FilmTypesPage';
import { FilmRecipesPage } from './film-recipes/FilmRecipesPage';
import { OrdersPage } from './orders/OrdersPage';
import { ProductionLinesPage } from './production-lines/ProductionLinesPage';

const AppContainer = styled(Box)({
	backgroundColor: '#F3F7FA',
	background: '#11101d',
	minHeight: '100vh',
	display: 'flex',
});

const ToastHost = () => (
	<ToastContainer
		position="top-right"
		autoClose={5000}
		hideProgressBar={false}
		newestOnTop={false}
		closeOnClick
		rtl={false}
		pauseOnFocusLoss
		draggable
		pauseOnHover
		theme="colored"/>
);

const WorkspaceContainer = styled(Box)({
	flexGrow: 1,
	p: 3,
	background: '#e4e9f7',
});

export const App = () => (
	<AppContainer>
		<ToastHost/>
		<SideMenu/>
		<WorkspaceContainer>
			<Routes>
				<Route path={CUSTOMERS} element={<CustomersPage/>}/>
				<Route path={`${CUSTOMERS}/:id`} element={<CustomersPage/>}/>
				<Route path={FILM_TYPES} element={<FilmTypesPage/>}/>
				<Route path={`${FILM_TYPES}/:id`} element={<FilmTypesPage/>}/>
				<Route path={FILM_RECIPES} element={<FilmRecipesPage/>}/>
				<Route path={`${FILM_RECIPES}/:id`} element={<FilmRecipesPage/>}/>
				<Route path={ORDERS} element={<OrdersPage/>}/>
				<Route path={`${ORDERS}/:id`} element={<OrdersPage/>}/>
				<Route path={PRODUCTION_LINES} element={<ProductionLinesPage/>}/>
				<Route path={`${PRODUCTION_LINES}/:id`} element={<ProductionLinesPage/>}/>
			</Routes>
		</WorkspaceContainer>
	</AppContainer>
)

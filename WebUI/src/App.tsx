import { SideMenu } from './menu/SideMenu';
import { Box, styled } from '@mui/material';
import { Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import { CUSTOMERS, FILM_RECIPES, FILM_TYPES } from './routes/routes';
import { CustomersPage } from './customers/CustomersPage';
import { FilmTypesPage } from './film-types/FilmTypesPage';
import { FilmRecipesPage } from './film-recipes/FilmRecipesPage';

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
			</Routes>
		</WorkspaceContainer>
	</AppContainer>
)

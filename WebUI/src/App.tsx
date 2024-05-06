import { SideMenu } from './menu/SideMenu';
import { Box, Typography, styled } from '@mui/material';
import { Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import { CUSTOMERS } from './routes/routes';
import { CustomersPage } from './customers/CustomersPage';

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
})

export const App = () => {
  return (
    <AppContainer>
      <ToastHost/>
      <SideMenu/>
      <WorkspaceContainer>
        <Routes>
          <Route path={CUSTOMERS} element={<CustomersPage/>}/>
          <Route path={`${CUSTOMERS}/:id`} element={<CustomersPage/>}/>
        </Routes>
      </WorkspaceContainer>
    </AppContainer>
  );
}

import React from 'react';
import ReactDOM from 'react-dom/client';
import reportWebVitals from './reportWebVitals';
import { App } from './App';
import { CssBaseline } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { ruRU } from '@mui/x-date-pickers/locales';
import 'dayjs/locale/ru';
import dayjs from "dayjs";
import updateLocale from 'dayjs/plugin/updateLocale';

dayjs.extend(updateLocale);

dayjs.updateLocale('ru', {
	months: [
		"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль",
		"Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь",
	],
});

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

root.render(
	<React.StrictMode>
		<CssBaseline/>
		<BrowserRouter>
			<LocalizationProvider
				dateAdapter={AdapterDayjs}
				localeText={ruRU.components.MuiLocalizationProvider.defaultProps.localeText}
				adapterLocale='ru'
			>
				<App/>
			</LocalizationProvider>
		</BrowserRouter>
	</React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

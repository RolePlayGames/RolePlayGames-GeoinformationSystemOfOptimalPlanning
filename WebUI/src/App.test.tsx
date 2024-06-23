import { App } from './App';
import renderer from 'react-test-renderer';
import { CssBaseline } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import { Fragment } from 'react/jsx-runtime';

test('App renders correctly', () => {
	// arrange & act
	const tree = renderer.create(
		<Fragment>
			<CssBaseline/>
			<BrowserRouter>
				<App/>
			</BrowserRouter>
		</Fragment>
	).toJSON();
  
	// assert
	expect(tree).toMatchSnapshot();
});
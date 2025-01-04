import { styled, Theme, CSSObject } from '@mui/material/styles';
import Box from '@mui/material/Box';
import MuiDrawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import Typography from '@mui/material/Typography';
import IconButton, { IconButtonProps } from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import { useState } from 'react';
import PublicOutlinedIcon from '@mui/icons-material/PublicOutlined';
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import { MenuItem } from './MenuItem';
import { white } from '../styles/colors';
import { menuItems } from './menuItems';

const drawerWidth = 280;

const openedMixin = (theme: Theme): CSSObject => ({
	width: drawerWidth,
	transition: theme.transitions.create('width', {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.enteringScreen,
	}),
	overflowX: 'hidden',
	background: '#11101d',
});

const closedMixin = (theme: Theme): CSSObject => ({
	width: `calc(${theme.spacing(7)} + 1px)`,
	overflowX: 'hidden',
	background: '#11101d',
	transition: theme.transitions.create('width', {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.leavingScreen,
	}),
	[theme.breakpoints.up('sm')]: {
		width: `calc(${theme.spacing(8)} + 1px)`,
	},
});

const MenuDrawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== 'open' })(
	({ theme, open }) => ({
		width: drawerWidth,
		boxSizing: 'border-box',
		flexShrink: 0,
		whiteSpace: 'nowrap',
		...(open && {
			...openedMixin(theme),
			'& .MuiDrawer-paper': openedMixin(theme),
		}),
		...(!open && {
			...closedMixin(theme),
			'& .MuiDrawer-paper': closedMixin(theme),
		}),
	}),
);

const MenuDrawerContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
	justifyContent: 'space-between',
	height: '100%',
});

const TopItemsContainer = styled(Box)({});

const OpenedMenuHeaderContainer = styled(Box)({
	padding: '8px',
	display: 'flex',
	flexDirection: 'row',
	alignItems: 'center',
	justifyContent: 'space-between',
	color: white,
	background: '#1d1b31',
});

const ApplicationInfoContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
});

const ClosedMenuHeaderContainer = styled(Box)({
	display: 'flex',
	alignItems: 'center',
	justifyContent: 'center',
	padding: '8px',
	background: '#1d1b31',
});

const ApplicationIcon = styled(PublicOutlinedIcon)({
	minHeight: 48,
	minWidth: 48,
});

const ApplicationName = styled(Typography)({
	minHeight: 48,
	marginLeft: '10px',
	display: 'flex',
	flexDirection: 'row',
	alignItems: 'center',
	fontSize: '24px',
});

const OpenMenuButton = styled((props: IconButtonProps) => (
	<IconButton {...props}>
		<MenuIcon />
	</IconButton>
))({
	color: white,
	minHeight: 48,
	minWidth: 48,
});

const CloseMenuButton = styled((props: IconButtonProps) => (
	<IconButton {...props}>
		<ChevronLeftIcon/>
	</IconButton>
))({
	color: white,
	minHeight: 48,
	minWidth: 48,
});

const MenuItemsList = styled(List)({
	background: '#11101d',
	color: white,
	padding: 0,
});

const BottomItemsContainer = styled(Box)({
	background: '#1d1b31',
	display: 'flex',
	flexDirection: 'row',
	color: white,
	alignItems: 'center',
	justifyContent: 'space-between',
	padding: '8px',
});

const ProfileInfoContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'row',
	alignItems: 'center',
});

const UserImage = styled(AccountBoxIcon)({
	minHeight: 48,
	minWidth: 48,
});

const UserNameAndRoleContainer = styled(Box)({
	display: 'flex',
	flexDirection: 'column',
	minHeight: 48,
	marginLeft: '10px'
});

const UserNameLabel = styled(Typography)({
	fontSize: '18px',
});

const UserRoleLabel = styled(Typography)({
	fontSize: '12px',
});

const LogoutButton = (props: IconButtonProps) => (
	<IconButton
		sx={{
			minHeight: 48,
			minWidth: 48,
			color: white ,
		}}
		{...props}
	>
		<LogoutOutlinedIcon/>
	</IconButton>
);

export const SideMenu = () => {
	const [isMenuOpen, setIsMenuOpen] = useState(false);

	const handleDrawerOpen = () => {
		setIsMenuOpen(true);
	};

	const handleDrawerClose = () => {
		setIsMenuOpen(false);
	};

	return (
		<MenuDrawer variant="permanent" open={isMenuOpen}>
			<MenuDrawerContainer>
				<TopItemsContainer>
					{ isMenuOpen ? (
						<OpenedMenuHeaderContainer>
							<ApplicationInfoContainer>
								<ApplicationIcon/>
								<ApplicationName>GSOP</ApplicationName>
							</ApplicationInfoContainer>
							<CloseMenuButton onClick={ handleDrawerClose }/>
						</OpenedMenuHeaderContainer>
					) : (
						<ClosedMenuHeaderContainer>
							<OpenMenuButton onClick={ handleDrawerOpen }/>
						</ClosedMenuHeaderContainer>
					)}
					<MenuItemsList>
						{ menuItems.map((item) => (
							<MenuItem key={item.header} item={item} isMenuOpen={isMenuOpen}/>
						))}
					</MenuItemsList>
				</TopItemsContainer>
				<BottomItemsContainer>
					<ProfileInfoContainer>
						<UserImage/>
						<UserNameAndRoleContainer>
							<UserNameLabel>User Name</UserNameLabel>
							<UserRoleLabel>User role</UserRoleLabel>
						</UserNameAndRoleContainer>
					</ProfileInfoContainer>
					<LogoutButton/>
				</BottomItemsContainer>
			</MenuDrawerContainer>
		</MenuDrawer>
	);
}
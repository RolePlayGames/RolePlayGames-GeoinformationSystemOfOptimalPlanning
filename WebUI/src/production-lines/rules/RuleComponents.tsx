import styled from "@emotion/styled";
import { Box } from "@mui/material";

export const RuleContainer = styled(Box)({ display: 'flex' });

export const BigFieldContainer = styled(Box)({ flexGrow: 7 });

export const FieldContainer = styled(Box)({ flexGrow: 4 });

export const DeleteButtonContainer = styled(Box)({
	display: 'flex',
	flexGrow: 1,
	alignItems: 'flex-start',
	justifyContent: 'center',
	marginTop: '13px',
});
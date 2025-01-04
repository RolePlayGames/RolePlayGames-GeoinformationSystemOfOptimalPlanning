import styled from "@emotion/styled";
import { Accordion, AccordionSummary, AccordionSummaryProps } from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

export const ChangeRuleList = styled(Accordion)({
	background: '#e4e9f7',
});

const ChangeRuleSummary = styled(AccordionSummary)({
	background: '#1d1b31',
	'&:hover': {
		backgroundColor: '#11101d'
	},
	color: 'white',
});

export const ChangeRuleHeader = (props: AccordionSummaryProps) => (
	<ChangeRuleSummary
		expandIcon={<ExpandMoreIcon sx={{ color: 'white' }}/>}
		{...props}/>
);
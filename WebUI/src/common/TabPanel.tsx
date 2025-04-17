import { Box } from "@mui/material";
import { removeMapFlag } from "./mapHelpers";
import { useEffect } from "react";

export interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}
  
export const TabPanel = ({ children, value, index, ...other }: TabPanelProps) => (
	<Box role="tabpanel" hidden={value !== index} {...other}>
		{value === index && <>{children}</>}
	</Box>
)
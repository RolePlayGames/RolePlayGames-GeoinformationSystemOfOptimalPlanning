import { useState, useCallback, useEffect } from "react";
import { Customer, getCustomer, } from "./customersClient";
import { CustomerElement } from "./CustomerElement";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";

type CustomerPageProps = {
    id: number,
    apiPath: string,
}

export const CustomerPage = ({ id, apiPath }: CustomerPageProps) => {

	const [item, setItem] = useState<Customer | undefined>();

	const navigate = useNavigate();
    
	const loadItem = useCallback(async () => {

		let item: Customer | undefined;

		if (id > 0) 
			item = await getCustomer(id);
		else 
			item = {
				name: '',
			}
        
        
		if (item)
			setItem(item);
		else
			navigate(apiPath);
	}, [id]);
    
	useEffect(() => {
		loadItem();
	}, [id]);

	if (item === undefined) 
		return (
			<LoadingProgress/>
		);
	else 
		return (
			<CustomerElement id={id} item={item} apiPath={apiPath}/>
		);
    
}
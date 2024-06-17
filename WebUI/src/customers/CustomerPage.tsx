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
    
    const loadCustomer = useCallback(async () => {

        let customer: Customer | undefined;

        if (id > 0) {
            customer = await getCustomer(id);
        } else {
            customer = {
                name: '',
            }
        }
        
        if (customer)
            setItem(customer);
        else
            navigate(apiPath);
    }, [id]);
    
    useEffect(() => {
        loadCustomer();
    }, [id]);

    if (item === undefined) {
        return (
            <LoadingProgress/>
        );
    } else {
        return (
            <CustomerElement id={id} item={item} apiPath={apiPath}/>
        );
    }
}
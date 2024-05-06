import { useState, useCallback, useEffect } from "react";
import { Customer, getCustomer, } from "./customersClient";
import { CustomerElement } from "./CustomerElement";
import { useNavigate } from "react-router-dom";
import { LoadingProgress } from "../common/LoadingProgress";

type CustomerPageProps = {
    id: number,
}

export const CustomerPage = ({ id }: CustomerPageProps) => {

    const [customer, setCustomer] = useState<Customer | undefined>();

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
            setCustomer(customer);
        else
            navigate('/customers');
    }, [id]);
    
    useEffect(() => {
        loadCustomer();
    }, [id]);

    if (customer === undefined) {
        return (
            <LoadingProgress/>
        );
    } else {
        return (
            <CustomerElement id={id} customer={customer}/>
        );
    }
}
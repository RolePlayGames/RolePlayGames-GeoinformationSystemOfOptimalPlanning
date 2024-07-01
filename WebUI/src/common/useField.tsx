import { useEffect, useState } from "react"

export type UseItemFieldProps<TItem, TField> = {
    item: TItem,
    valueFactory: (item: TItem) => TField,
    validationFactory: (value: TField) => string,
}

export function useItemField<TItem, TField>({ item, valueFactory, validationFactory }: UseItemFieldProps<TItem, TField>) {
    const [field, setField] = useState(valueFactory(item));
    const [errorText, setErrorText] = useState<string>();

    useEffect(() => {
        setErrorText(validationFactory(field));
    }, [field]);

    useEffect(() => {
        setField(valueFactory(item));
    }, [item]);

    return [field, setField, errorText] as const;
}
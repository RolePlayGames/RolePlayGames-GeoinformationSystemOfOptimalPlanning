import { useEffect, useState } from "react"

export type UseItemFieldProps<TItem, TField> = {
    item: TItem,
    valueFactory: (item: TItem) => TField,
    validationFactory: (value: TField) => string | undefined,
}

export function useItemField<TItem, TField>(item: TItem, valueFactory: (item: TItem) => TField) {
	const [field, setField] = useState(valueFactory(item));

	useEffect(() => {
		setField(valueFactory(item));
	}, [item]);

	return [field, setField] as const;
}

export function useItemFieldWithValidation<TItem, TField>(item: TItem, valueFactory: (item: TItem) => TField, validationFactory: (value: TField) => string | undefined) {
	const [field, setField] = useState(valueFactory(item));
	const [errorText, setErrorText] = useState<string>();

	useEffect(() => {
		setErrorText(validationFactory(field));
	}, [field]);

	useEffect(() => {
		setField(valueFactory(item));
	}, [item]);

	return [field, setField, errorText, setErrorText] as const;
}
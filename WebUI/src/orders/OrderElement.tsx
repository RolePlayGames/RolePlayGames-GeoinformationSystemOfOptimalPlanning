import { useNavigate } from "react-router-dom";
import { HeaderLabel } from "../common/controls";
import { ElementContainer, ActionsBar, SaveButton, DeleteButton } from "../common/elementControls";
import { Order, updateOrder, createOrder, deleteOrder } from "./ordersClient";
import { convertToInt, convertToNumber } from "../utils/number-converters/numberConverter";
import { useItemField, useItemFieldWithValidation } from "../common/useItemField";
import { validateFinishedGoods, validateNumber, validatePriceOverdue, validateQuantityInRunningMeter, validateRollsCount, validateWaste, validateWidth } from "./validations";
import { CustomerInfo } from "../customers/customersClient";
import { FilmRecipeInfo } from "../film-recipes/filmRecipesClient";
import dayjs, { Dayjs } from "dayjs";
import { IClientError } from "../common/clients/clientError";
import { DateTimeField, InputField, SelectField } from "../common/inputs";
import { toast } from "react-toastify";

type OrderElementProps = {
    id: number,
    item: Order,
    apiPath: string,
	customers: CustomerInfo[],
	filmRecipes: FilmRecipeInfo[],
}

export const OrderElement = ({ id, item, apiPath, customers, filmRecipes }: OrderElementProps) => {
	const [number, setNumber, numberError, setNumberError] = useItemFieldWithValidation<Order, string>(item, x => x.number, validateNumber);
	const [customerID, setCustomerID] = useItemField<Order, number | undefined>(item, item => customers.findIndex(x => x.id === item.customerID) > -1 ? item.customerID : customers[0].id);
	const [filmRecipeID, setFilmRecipeID] = useItemField<Order, number | undefined>(item, item => filmRecipes.findIndex(x => x.id === item.filmRecipeID) > -1 ? item.filmRecipeID : filmRecipes[0].id);
	const [width, setWidth, widthError] = useItemFieldWithValidation<Order, string>(item, x => x.width.toString(), validateWidth);
	const [quantityInRunningMeter, setQuantityInRunningMeter, quantityInRunningMeterError] = useItemFieldWithValidation<Order, string>(item, x => x.quantityInRunningMeter.toString(), validateQuantityInRunningMeter);
	const [finishedGoods, setFinishedGoods, finishedGoodsError] = useItemFieldWithValidation<Order, string>(item, x => x.finishedGoods.toString(), validateFinishedGoods);
	const [waste, setWaste, wasteError] = useItemFieldWithValidation<Order, string>(item, x => x.waste.toString(), validateWaste);
	const [rollsCount, setRollsCount, rollsCountError] = useItemFieldWithValidation<Order, string>(item, x => x.rollsCount.toString(), validateRollsCount);
	const [plannedDate, setPlannedDate] = useItemField<Order, Dayjs | null>(item, x => dayjs(x.plannedDate));
	const [priceOverdue, setPriceOverdue, priceOverdueError] = useItemFieldWithValidation<Order, string>(item, x => x.priceOverdue.toString(), validatePriceOverdue);

	const navigate = useNavigate();

	const onUpdate = async (item: Order) => {
		try {
			await updateOrder(id, item);
			navigate(apiPath);
			toast.success(`Заказ ${number} был обновлен`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'OrderNumberAlreadyExistsException') {
				toast.error(`Произошла ошибка при обновлении: указанный номер уже используется в другом заказе`);
				setNumberError('Указанный номер уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при обновлении. Проверьте параметры`);
			
		}
	};

	const onCreate = async (item: Order) => {
		try {
			await createOrder(item);
			navigate(apiPath);
			toast.success(`Заказ ${number} был создан`);
		} catch (error: unknown) {
			if ((error as IClientError).errorCode === 'OrderNumberAlreadyExistsException') {
				toast.error(`Произошла ошибка при создании: указанный номер уже используется в другом заказе`);
				setNumberError('Указанный номер уже используется в системе');
			} else 
				toast.error(`Произошла ошибка при создании. Проверьте параметры`);
			
		}
	};

	const onSave = () => {
		const widthNumber = convertToInt(width);
		const quantityInRunningMeterNumber = convertToInt(quantityInRunningMeter);
		const finishedGoodsNumber = convertToNumber(finishedGoods);
		const wasteNumber = convertToNumber(waste);
		const rollsCountNumber = convertToInt(rollsCount);
		const priceOverdueNumber = convertToNumber(priceOverdue);

		if (widthNumber !== undefined
			&& quantityInRunningMeterNumber !== undefined
			&& finishedGoodsNumber !== undefined
			&& wasteNumber !== undefined
			&& rollsCountNumber !== undefined
            && plannedDate !== null
			&& priceOverdueNumber !== undefined
			&& customerID !== undefined
			&& filmRecipeID !== undefined)
		{
			const item = {
				number: number,
				customerID: customerID,
				filmRecipeID: filmRecipeID,
				width: widthNumber,
				quantityInRunningMeter: quantityInRunningMeterNumber,
				finishedGoods: finishedGoodsNumber,
				waste: wasteNumber,
				rollsCount: rollsCountNumber,
				plannedDate: plannedDate.toDate(),
				priceOverdue: priceOverdueNumber,
			};

			if (id > 0) 
				return onUpdate(item);
			else 
				return onCreate(item);
		}
		
		return Promise.resolve();
	};

	const onDelete = async () => {
		if (id > 0) {
			await deleteOrder(id);
			navigate(apiPath);
		}
	};

	return(
		<ElementContainer>
			<HeaderLabel>Заказ {item.number}</HeaderLabel>
			<ActionsBar>
				<SaveButton
					onClick={onSave}
					disabled={
						!!numberError
						&& !!widthError
						&& !!quantityInRunningMeterError
						&& !!finishedGoodsError
						&& !!wasteError
						&& !!rollsCountError
						&& plannedDate === null
						&& !!priceOverdueError
						&& customerID !== undefined
						&& filmRecipeID !== undefined
					}
				/>
				<DeleteButton onClick={onDelete} disabled={id <= 0}/>
			</ActionsBar>
			<InputField
				label='Номер'
				value={number}
				onChange={setNumber}
				errorText={numberError}/>
			<SelectField
				label='Заказчик'
				items={customers}
				value={customerID}
				onItemChanged={setCustomerID}
				variant='standard'
			/>
			<SelectField
				label='Рецепт пленки'
				items={filmRecipes}
				value={filmRecipeID}
				onItemChanged={setFilmRecipeID}
				variant='standard'
			/>
			<InputField
				label='Ширина, мм'
				value={width}
				onChange={setWidth}
				errorText={widthError}/>
			<InputField
				label='Количество в погонном метре, м'
				value={quantityInRunningMeter}
				onChange={setQuantityInRunningMeter}
				errorText={quantityInRunningMeterError}/>
			<InputField
				label='Количество выходного материала, кг'
				value={finishedGoods}
				onChange={setFinishedGoods}
				errorText={finishedGoodsError}/>
			<InputField
				label='Расход, кг'
				value={waste}
				onChange={setWaste}
				errorText={wasteError}/>
			<InputField
				label='Количество рулонов, шт'
				value={rollsCount}
				onChange={setRollsCount}
				errorText={rollsCountError}/>
			<DateTimeField
				label='Планируемая дата'
				value={plannedDate}
				onChange={setPlannedDate}
			/>
			<InputField
				label='Штраф за нарушение сроков, у.е./час'
				value={priceOverdue}
				onChange={setPriceOverdue}
				errorText={priceOverdueError}/>
		</ElementContainer>
	);
}
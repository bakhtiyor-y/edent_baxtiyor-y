import { InventoryIncomeItemModel } from './inventory-income-item.model';

export interface InventoryIncomeModel {
    id: number;
    createdDate: Date;
    totalCost: number;
    description: string;
    who: string;
    inventoryItems: InventoryIncomeItemModel[];
}

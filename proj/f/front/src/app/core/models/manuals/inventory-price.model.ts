import { InventoryModel } from './inventory.model';

export interface InventoryPriceModel {
    id: number;
    price: number;
    dateFrom: Date;
    inventoryId: number;
    inventory: InventoryModel;
}

import { InventoryModel } from '../manuals';

export interface ReceptInventoryModel {
    id: number;
    quantity: number;
    inventoryId: number;
    inventory: InventoryModel;
    measurementUnitId: number;
    receptId: number;
}

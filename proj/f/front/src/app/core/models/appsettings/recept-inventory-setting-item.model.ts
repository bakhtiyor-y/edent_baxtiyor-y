import { InventoryModel, MeasurementUnitModel } from '../manuals';

export interface ReceptInventorySettingItemModel {
    id: number;
    quantity: number;
    inventoryId: number;
    // public inventory: InventoryModel;
    receptInventorySettingId: number;
    measurementUnitId: number;
    measurementUnit: MeasurementUnitModel;
    selectedInventory: InventoryModel;
}

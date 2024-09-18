import { InventoryModel, MeasurementUnitModel } from '../manuals';

export interface InventoryOutcomeItemModel {
    id: number;
    quantity: number;
    measurementUnitId: number;
    measurementUnit: MeasurementUnitModel;
    inventoryIncomeId: number;
    inventoryId: number;
    inventory: InventoryModel;
}

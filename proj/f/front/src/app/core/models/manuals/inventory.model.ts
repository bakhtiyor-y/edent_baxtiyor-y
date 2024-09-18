import { InventoryPriceModel } from './inventory-price.model';
import { MeasurementUnitTypeModel } from './measurement-unit-type.model';
import { MeasurementUnitModel } from './measurement-unit.model';

export interface InventoryModel {
    id: number;
    name: string;
    measurementUnitId: number;
    measurementUnitTypeId: number;
    stock: number;
    currentPrice: number;
    measurementUnit: MeasurementUnitModel;
    measurementUnitType: MeasurementUnitTypeModel;
    inventoryPrices: InventoryPriceModel[];
}

import { MeasurementUnitModel } from './measurement-unit.model';

export interface MeasurementUnitTypeModel {
    id: number;
    name: string;
    code: string;
    measurementUnits: MeasurementUnitModel[];
}

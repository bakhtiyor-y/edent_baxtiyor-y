import { MeasurementUnitTypeModel } from './measurement-unit-type.model';

export interface MeasurementUnitModel {
    id: number;
    name: string;
    code: string;
    multiplicity: number;
    deafault: boolean;
    measurementUnitTypeId: number;
    measurementUnitType: MeasurementUnitTypeModel;

}

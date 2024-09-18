import { DiagnoseModel } from '../manuals';
import { PatientToothModel } from './patient-tooth.model';
import { ReceptModel } from './recept.model';
import { TreatmentDentalServiceModel } from './treatment-dental-service.model';

export interface TreatmentModel {
    receptId: number;
    recept: ReceptModel;
    diagnoseId?: number;
    diagnose: DiagnoseModel;
    patientToothId: number;
    patientTooth: PatientToothModel;
    treatmentDentalServices: TreatmentDentalServiceModel[];
    description: string;
}

import { ToothModel } from '../appsettings';
import { PatientToothModel } from './patient-tooth.model';
import { TreatmentModel } from './treatment.model';

export interface PatientToothTreatmentModel {
    id: number;
    treatmentId: number;
    patientToothId: number;
    patientTooth: PatientToothModel;
    treatment: TreatmentModel;
}

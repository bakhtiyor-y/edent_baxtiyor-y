import { DentalServiceModel } from '../manuals';
import { TreatmentModel } from './treatment.model';

export interface TreatmentDentalServiceModel {
    id: number;
    treatmentId: number;
    dentalServiceId: number;
    dentalService: DentalServiceModel;
    treatment: TreatmentModel;
}

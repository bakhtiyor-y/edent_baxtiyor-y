import { DoctorModel, PatientModel } from '../user-management';
import { AppointmentModel } from './appointment.model';
import { ReceptDentalServiceModel } from './recept-dental-service.model';
import { ReceptInventoryModel } from './recept-inventory.model';
import { TreatmentModel } from './treatment.model';

export interface ReceptModel {
    id: number;
    patientId: number;
    doctorId?: number;
    employeeId?: number;
    appointmentId: number;
    patient: PatientModel;
    doctor: DoctorModel;
    appointment: AppointmentModel;
    receptInventories: ReceptInventoryModel[];
    treatments: TreatmentModel[];
    receptDentalServices: ReceptDentalServiceModel[];
    description: string;
    createdDate: Date;
}

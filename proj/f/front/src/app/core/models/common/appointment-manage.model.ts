import { AppointmentStatus } from '../../enums';
import { PatientManageModel } from '../user-management';

export interface AppointmentManageModel {
    id: number;
    appointmentDate: Date;
    appointmentStatus: AppointmentStatus;
    partnerId?: number;
    description: string;
    patient: PatientManageModel;
    dentalChairId?: number;
    jointDoctors: number[];
    employeeId?: number;
    doctorId?: number;
}

import { AppointmentStatus } from '../../enums';
import { PatientManageModel } from '../user-management';

export interface AppointmentManageWithLastModel {
    id: number;
    appointmentDate: Date;
    appointmentDateLast: Date;
    appointmentStatus: AppointmentStatus;
    partnerId?: number;
    description: string;
    patient: PatientManageModel;
    dentalChairId?: number;
    jointDoctors: number[];
    employeeId?: number;
    doctorId?: number;
}

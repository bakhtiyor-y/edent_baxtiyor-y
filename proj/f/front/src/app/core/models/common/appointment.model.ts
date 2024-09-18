import { AppointmentStatus } from '../../enums';

export interface AppointmentModel {
    id: number;
    appointmentDate: Date;
    appointmentStatus: AppointmentStatus;
    patientFullName: string;
    patientBirthDate: Date;
    patientPhoneNumber: string;
    doctorFullName: string;
    description: string;
    doctorId?: number;
    employeeId?: number;
    patientId: number;
    partnerId?: number;
}

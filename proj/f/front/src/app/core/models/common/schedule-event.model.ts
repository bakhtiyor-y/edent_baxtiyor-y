import { AppointmentStatus } from "../../enums";

export interface ScheduleEventModel {
    name: string;
    description: string;
    isBusy: boolean;
    starting: Date;
    startingText: string;
    appointmentId: number;
    appointmentStatus: AppointmentStatus
}

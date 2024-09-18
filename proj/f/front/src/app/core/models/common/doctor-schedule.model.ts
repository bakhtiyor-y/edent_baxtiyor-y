import { ScheduleEventModel } from "./schedule-event.model";

export interface DoctorScheduleModel {
    name: string;
    admissionDuration: number;
    events: ScheduleEventModel[];
}
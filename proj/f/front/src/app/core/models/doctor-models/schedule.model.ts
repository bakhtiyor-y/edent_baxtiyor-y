import { ScheduleSettingModel } from './schedule-setting.model';

export interface ScheduleModel {
    id: number;
    doctorId: number;
    isActive: boolean;
    admissionDuration: number;
    fromDate: Date;
    toDate: Date;
    scheduleSettings: ScheduleSettingModel[];
}

import { SettingDayOfWeekModel } from './setting-day-of-week.model';

export interface ScheduleSettingModel {
    id: number;
    fromMinute: Date;
    toMinute: Date;
    scheduleId: number;
    settingDayOfWeeks: SettingDayOfWeekModel[];
}

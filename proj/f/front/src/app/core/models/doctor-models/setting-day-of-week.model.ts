import { DayOfWeek } from '../../enums';

export interface SettingDayOfWeekModel {
    id: number;
    dayOfWeek: DayOfWeek;
    scheduleSettingId: number;
}

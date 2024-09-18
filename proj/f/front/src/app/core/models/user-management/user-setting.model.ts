import { UserModel } from './user.model';

export interface UserSettingModel {
    id: number;
    userId: number;
    key: string;
    value: any;
    user: UserModel;
}

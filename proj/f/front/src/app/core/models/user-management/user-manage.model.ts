import { Gender } from "../../enums";

export interface UserManageModel {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    patronymic: string;
    birthDate: Date;
    phoneNumber: string;
    email: string;
    isActive: boolean;
    profileImage: string;
    roles: string[];
    gender: Gender;
}

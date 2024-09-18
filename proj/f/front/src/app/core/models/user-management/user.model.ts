import { Gender } from "../../enums";

export interface UserModel {
    id: number;
    employeeId?: number;
    userName: string;
    firstName: string;
    lastName: string;
    patronymic: string;
    fullName: string;
    birthDate: Date;
    phoneNumber: string;
    email: string;
    isActive: boolean;
    gender: Gender;
    profileImage: string;
}

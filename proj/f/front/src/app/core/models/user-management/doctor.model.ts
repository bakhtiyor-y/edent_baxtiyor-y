import { Gender } from "../../enums";

export interface DoctorModel {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    birthDate: Date;
    isActive: boolean;
    profileImage: string;
    fullName: string;
    email: string;
    gender: Gender;
    specialization: string;
    phoneNumber: string;
    term: string;
}

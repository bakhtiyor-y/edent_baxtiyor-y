import { Gender } from "../../enums";

export interface TechnicModel {
    id: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    birthDate: Date;
    fullName: string;
    phoneNumber: string;
    gender: Gender;
}

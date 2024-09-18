import { Gender } from "../../enums";

export interface DoctorManageModel {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    birthDate: Date;
    isActive: boolean;
    email: string;
    phoneNumber: string;
    specializationId: number;
    termId: number;
    termValue: number;
    countryId: number;
    regionId: number;
    cityId: number;
    dentalChairs: [];
    addressLine1: string;
    addressLine2: string;
    gender: Gender;
}

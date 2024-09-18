import { Gender, PatientAgeType } from "../../enums";

export interface PatientModel {
    id: number;
    userId: number;
    fullName: string;
    birthDate: Date;
    email: string;
    phoneNumber: string;
    address: string;
    gender: Gender;
    patientAgeType: PatientAgeType;
}

import { PatientAgeType, ToothType } from "../../enums";

export interface PatientManageModel {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    birthDate: Date;
    email: string;
    phoneNumber: string;
    countryId: number;
    regionId: number;
    cityId: number;
    addressLine1: string;
    addressLine2: string;
    patientAgeType: PatientAgeType;
}

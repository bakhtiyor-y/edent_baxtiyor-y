import { PatientAgeType } from "../../enums";
import { PatientToothModel } from "../common/patient-tooth.model";

export interface Teeth {
    patientId: number;
    patientAgeType: PatientAgeType;
    topLeft: PatientToothModel[];
    topRight: PatientToothModel[];
    bottomLeft: PatientToothModel[];
    bottomRight: PatientToothModel[];
}

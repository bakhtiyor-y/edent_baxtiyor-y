import { ToothState } from "../../enums";
import { ToothModel } from "../appsettings";

export interface PatientToothModel {
    id: number;
    patientId: number;
    toothId: number;
    toothState: ToothState,
    tooth: ToothModel;
}
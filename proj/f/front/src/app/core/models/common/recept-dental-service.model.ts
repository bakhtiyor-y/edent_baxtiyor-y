import { DentalServiceModel } from "../manuals/dental-service.model";
import { ReceptModel } from "./recept.model";

export interface ReceptDentalServiceModel {
    id: number;
    treatmentId: number;
    dentalServiceId: number;
    dentalService: DentalServiceModel;
    recept: ReceptModel;
}

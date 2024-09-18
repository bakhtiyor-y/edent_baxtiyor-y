import { DentalServiceModel } from './dental-service.model';

export interface DentalServiceGroupModel {
    id: number;
    name: string;
    description: string;
    dentalServices: DentalServiceModel[];
}

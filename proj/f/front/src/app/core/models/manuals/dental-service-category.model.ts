import { DentalServiceModel } from './dental-service.model';

export interface DentalServiceCategoryModel {
    id: number;
    name: string;
    description: string;
    dentalServices: DentalServiceModel[];
}


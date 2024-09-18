import { DentalServiceModel } from './dental-service.model';

export interface DentalServicePriceModel {
    id: number;
    price: number;
    dateFrom: Date;
    dentalServiceId: number;
    dentalService: DentalServiceModel;
}

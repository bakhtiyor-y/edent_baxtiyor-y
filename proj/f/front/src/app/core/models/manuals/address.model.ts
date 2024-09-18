import { CityModel } from './city.model';

export interface AddressModel {
    id: number;
    addressLine1: string;
    addressLine2: string;
    cityId: number;
    city: CityModel;
}

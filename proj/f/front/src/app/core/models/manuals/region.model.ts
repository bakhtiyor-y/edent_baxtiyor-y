import { CountryModel } from './country.model';

export interface RegionModel {
    id: number;
    name: string;
    code: string;
    countryId: number;
    country: CountryModel;
}

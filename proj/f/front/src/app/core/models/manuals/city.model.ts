import { RegionModel } from './region.model';

export interface CityModel {
    id: number;
    name: string;
    code: string;
    regionId: number;
    region: RegionModel;
}

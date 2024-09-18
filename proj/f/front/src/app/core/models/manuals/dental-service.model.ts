import { DentalServiceType, ToothState } from '../../enums';
import { DentalServiceCategoryModel } from './dental-service-category.model';
import { DentalServiceGroupModel } from './dental-service-group.model';

export interface DentalServiceModel {
    id: number;
    name: string;
    description: string;
    currentPrice: number;
    categoryName: string;
    type: DentalServiceType;
    toothState: ToothState;
    groupName: string;
    dentalServiceGroupId: number;
    dentalServiceCategoryId?: number;
    dentalServiceGroup: DentalServiceGroupModel;
    dentalServiceCategory: DentalServiceCategoryModel;
    receptInventorySettings: number[];
}

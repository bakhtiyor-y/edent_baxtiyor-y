import { ReceptInventorySettingItemModel } from './recept-inventory-setting-item.model';

export interface ReceptInventorySettingModel {
    id: number;
    name: string;
    isActive: boolean;
    isDefault: boolean;
    receptInventorySettingItems: ReceptInventorySettingItemModel[];
}

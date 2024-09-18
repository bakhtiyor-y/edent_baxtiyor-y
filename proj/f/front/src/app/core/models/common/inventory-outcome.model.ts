import { InventoryOutcomeItemModel } from './inventory-outcome-item.model';

export interface InventoryOutcomeModel {
    id: number;
    receptId?: number;
    createdDate: Date;
    totalCost: number;
    description: string;
    who: string;
    whom: string;
    recipientId: string;
    inventoryItems: InventoryOutcomeItemModel[];
}

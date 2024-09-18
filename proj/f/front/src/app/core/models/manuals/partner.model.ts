import { ProfitType } from '../../enums';

export interface PartnerModel {
    id: number;
    name: string;
    description: string;
    profitType: ProfitType;
    profit: number;
}

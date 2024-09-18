import { DiscountType } from '../../enums';

export interface DiscountModel {
    invoiceId: number;
    discount: number;
    discountType: DiscountType;
}

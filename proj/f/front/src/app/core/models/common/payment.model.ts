import { PaymentType } from '../../enums';

export interface PaymentModel {
    id: number;
    invoiceId: number;
    payedSumm: number;
    paymentType: PaymentType;
    isFromBalance: boolean;
    createdDate: Date;
}

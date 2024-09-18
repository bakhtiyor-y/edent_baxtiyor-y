export interface InvoiceModel {
    id: number;
    receptId: number;
    totalSumm: number;
    discount: number;
    providedSumm: number;
    debt: number;
    patientBalance: number;
    patientFullName: string;
    doctorFullName: string;
    phoneNumber: string;
    receptDate: Date;
}

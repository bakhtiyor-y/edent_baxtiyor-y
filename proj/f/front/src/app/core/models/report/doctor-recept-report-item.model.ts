export interface DoctorReceptReportItem {
    id: number;
    receptDate: Date;
    patientName: string;
    discount: number;
    totalIncome: number;
    doctorOutcome: number;
    technicOutcome: number;
    partnerShare: number;
    calculated: number;
    left: number;
}

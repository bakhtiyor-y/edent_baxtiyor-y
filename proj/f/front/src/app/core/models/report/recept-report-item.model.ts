export interface ReceptReportItem {
    id: number;
    receptDate: Date;
    doctorName: string;
    patientName: string;
    clinicIncome: number;
    discount: number;
    totalIncome: number;
    clinicOutcome: number;
    doctorOutcome: number;
    technicOutcome: number;
    partnerShare: number;
    profit: number;
    isDoctorCalculated: boolean;
    isPartnerCalculated: boolean;
    isTechnicCalculated: boolean;
}

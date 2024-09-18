import { DoctorReceptReportItem } from './doctor-recept-report-item.model';
export interface DoctorReceptReport {
    items: DoctorReceptReportItem[];
    totalDiscount: number;
    totalIncome: number;
    totalDoctorOutcome: number;
    totalTechnicOutcome: number;
    totalPartnerShare: number;
    totalCalculated: number;
    totalLeft: number;
}

import { ReceptReportItem } from './recept-report-item.model';
export interface ReceptReport {
    items: ReceptReportItem[];
    totalClinicIncome: number;
    totalDiscount: number;
    totalIncome: number;
    totalClinicOutcome: number;
    totalDoctorOutcome: number;
    totalTechnicOutcome: number;
    totalPartnerShare: number;
    totalProfit: number;
}

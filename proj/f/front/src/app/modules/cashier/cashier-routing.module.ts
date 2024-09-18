import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PatientsReportComponent } from '../reports/patients-report/patients-report.component';
import { CashierDashboardComponent } from '../shared/components/dashboard/cashier-dashboard/cashier-dashboard.component';
import { InvoiceComponent } from './invoice/invoice.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', component: CashierDashboardComponent },
            { path: 'invoice', component: InvoiceComponent },
            { path: 'patients-report', component: PatientsReportComponent }
        ])
    ],
    exports: [RouterModule]
})
export class CashierRoutingModule { }

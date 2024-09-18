import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminGuard } from 'src/app/core/services/guard-services';
import { DoctorsReportComponent } from './doctors-report/doctors-report.component';
import { ReceptReportComponent } from './recept-report/recept-report.component';
import { PartnersReportComponent } from './partners-report/partners-report.component';
import { DentalServiceReportComponent } from './dental-service-report/dental-service-report.component';
import { PatientsReportComponent } from './patients-report/patients-report.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: '', redirectTo: 'income' },
                    { path: 'recept-report', component: ReceptReportComponent },
                    { path: 'partners-report', component: PartnersReportComponent },
                    { path: 'doctors-report', component: DoctorsReportComponent },
                    { path: 'patients-report', component: PatientsReportComponent },
                    { path: 'dental-service-report', component: DentalServiceReportComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class ReportsRoutingModule { }

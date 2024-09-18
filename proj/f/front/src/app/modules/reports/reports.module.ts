import { NgModule } from '@angular/core';
import { ReportsRoutingModule } from './reports-routing.module';
import { ReceptReportComponent } from './recept-report/recept-report.component';
import { PartnersReportComponent } from './partners-report/partners-report.component';
import { DoctorsReportComponent } from './doctors-report/doctors-report.component';
import { SharedModule } from '../shared/shared.module';
import { DentalServiceReportComponent } from './dental-service-report/dental-service-report.component';
import { PatientsReportComponent } from './patients-report/patients-report.component';
import { PatientInvoicesComponent } from './patients-report/patient-invoices/patient-invoices.component';



@NgModule({
  declarations: [
    ReceptReportComponent,
    PartnersReportComponent,
    DoctorsReportComponent,
    DentalServiceReportComponent,
    PatientsReportComponent,
    PatientInvoicesComponent
  ],
  imports: [ReportsRoutingModule, SharedModule],
  exports: [PatientsReportComponent]
})
export class ReportsModule { }

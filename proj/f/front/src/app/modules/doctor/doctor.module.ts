import { NgModule } from '@angular/core';
import { DoctorRoutingModule } from './doctor-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ScheduleComponent } from './schedule/schedule.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { ReceptComponent } from './recept/recept.component';
import { ReportComponent } from './report/report.component';
import { ReceptFormComponent } from './recept-form/recept-form.component';
import { DoctorAppointPatientComponent } from './doctor-appoint-patient/doctor-appoint-patient.component';
import { MyPatientsComponent } from './my-patients/my-patients.component';
import { DoctorEditAppointComponent } from './doctor-edit-appoint/doctor-edit-appoint.component';



@NgModule({
  declarations: [
    ScheduleComponent,
    AppointmentComponent,
    ReceptComponent,
    ReportComponent,
    ReceptFormComponent,
    DoctorAppointPatientComponent,
    MyPatientsComponent,
    DoctorEditAppointComponent
  ],
  imports: [
    DoctorRoutingModule,
    SharedModule
  ]
})
export class DoctorModule { }

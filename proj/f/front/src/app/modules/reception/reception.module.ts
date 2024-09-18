import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ReceptionRoutingModule } from './reception-routing.module';
import { AppointPatientComponent } from './appoint-patient/appoint-patient.component';
import { PatientAppointmentsComponent } from './patient-appointments/patient-appointments.component';
import { EditAppointmentComponent } from './edit-appointment/edit-appointment.component';
import { DoctorsScheduleComponent } from './doctors-schedule/doctors-schedule.component';
import { AppointPatientOnBoardComponent } from './appoint-patient-on-board/appoint-patient-on-board.component';
import { RentgenModule } from '../rentgen/rentgen.module';



@NgModule({
  declarations: [
    AppointPatientComponent,
    PatientAppointmentsComponent,
    EditAppointmentComponent,
    DoctorsScheduleComponent,
    AppointPatientOnBoardComponent
  ],
  imports: [
    ReceptionRoutingModule,
    SharedModule,
    RentgenModule
  ]
})
export class ReceptionModule { }

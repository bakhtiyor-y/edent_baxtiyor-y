import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PatientsComponent } from '../shared/components/patients/patients.component';
import { AppointPatientComponent } from './appoint-patient/appoint-patient.component';
import { DoctorsScheduleComponent } from './doctors-schedule/doctors-schedule.component';
import { PatientAppointmentsComponent } from './patient-appointments/patient-appointments.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', redirectTo: 'doctors-schedule' },
            { path: 'doctors-schedule', component: DoctorsScheduleComponent },
            { path: 'appointments', component: PatientAppointmentsComponent },
            { path: 'new-appointment/:id', component: AppointPatientComponent },
            { path: 'patient', component: PatientsComponent },
            { path: 'rentgen', loadChildren: () => import('../rentgen/rentgen.module').then(m => m.RentgenModule) }
        ])
    ],
    exports: [RouterModule]
})
export class ReceptionRoutingModule { }

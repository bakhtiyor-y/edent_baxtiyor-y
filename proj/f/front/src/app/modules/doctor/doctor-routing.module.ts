import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DoctorDashboardComponent } from '../shared/components/dashboard/doctor-dashboard/doctor-dashboard.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { MyPatientsComponent } from './my-patients/my-patients.component';
import { ReceptFormComponent } from './recept-form/recept-form.component';
import { ReceptComponent } from './recept/recept.component';
import { ReportComponent } from './report/report.component';
import { ScheduleComponent } from './schedule/schedule.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', redirectTo: 'appointment' },
            { path: 'appointment', component: AppointmentComponent },
            { path: 'recept-form/:id', component: ReceptFormComponent },
            { path: 'my-patients', component: MyPatientsComponent },
            { path: 'recept', component: ReceptComponent },
            { path: 'schedule', component: ScheduleComponent },
            { path: 'report', component: ReportComponent }

        ])
    ],
    exports: [RouterModule]
})
export class DoctorRoutingModule { }

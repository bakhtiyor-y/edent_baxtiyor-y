import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { CashierDashboardComponent } from './cashier-dashboard/cashier-dashboard.component';
import { DoctorDashboardComponent } from './doctor-dashboard/doctor-dashboard.component';
import { ReceptionDashboardComponent } from './reception-dashboard/reception-dashboard.component';
import { ChartModule } from 'primeng/chart';
import { CalendarModule } from 'primeng/calendar';
import { DashboardRoutingModule } from './dashboard-routing.module';


@NgModule({
    declarations: [AdminDashboardComponent,
        DoctorDashboardComponent,
        ReceptionDashboardComponent,
        CashierDashboardComponent],
    imports: [
        DashboardRoutingModule,
        CalendarModule,
        SharedModule,
        ChartModule
    ]
})
export class DashboardModule { }

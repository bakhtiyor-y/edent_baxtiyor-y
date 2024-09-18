
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminGuard } from 'src/app/core/services/guard-services';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';

@NgModule({
    imports: [
        RouterModule.forChild([

            { path: '', component: AdminDashboardComponent, canActivate: [AdminGuard] }
        ])
    ],
    exports: [RouterModule]
})
export class DashboardRoutingModule { }

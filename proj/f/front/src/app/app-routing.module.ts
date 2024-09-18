import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppNotfoundComponent } from './core/components/pages/app.notfound.component';
import { AppErrorComponent } from './core/components/pages/app.error.component';
import { AppAccessdeniedComponent } from './core/components/pages/app.accessdenied.component';
import { AppMainComponent } from './layouts/app.main.component';
import { AppLoginComponent } from './authentication/app.login.component';
import { LayoutComponent } from './layouts/layout/layout.component';
import { AdminGuard } from './core/services/guard-services/admin-guard.service';
import { NoAuthGuard, PermissionGuard, RentgenGuard } from './core/services/guard-services';
import { ProfileComponent } from './modules/profile/profile.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            { path: '', component: AppMainComponent },
            {
                path: 'dashboard',
                component: LayoutComponent,
                data: {
                    title: 'Администрирование'
                },
                children: [
                    {
                        path: 'admin',
                        loadChildren: () => import('./modules/shared/components/dashboard/dashboard.module').then(m => m.DashboardModule),
                        canActivate: [AdminGuard]
                    },
                    {
                        path: 'user-management',
                        loadChildren: () => import('./modules/user-management/user-management.module').then(m => m.UserManagementModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'inventory-management',
                        loadChildren: () => import('./modules/inventory-management/inventory-management.module')
                            .then(m => m.InventoryManagementModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'manuals',
                        loadChildren: () => import('./modules/manuals/manuals.module').then(m => m.ManualsModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'app-settings',
                        loadChildren: () => import('./modules/app-settings/app-settings.module').then(m => m.AppSettingsModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'reports',
                        loadChildren: () => import('./modules/reports/reports.module').then(m => m.ReportsModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'reception',
                        loadChildren: () => import('./modules/reception/reception.module').then(m => m.ReceptionModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'cashier',
                        loadChildren: () => import('./modules/cashier/cashier.module').then(m => m.CashierModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'doctor',
                        loadChildren: () => import('./modules/doctor/doctor.module').then(m => m.DoctorModule),
                        canActivate: [PermissionGuard]
                    },
                    {
                        path: 'rentgen',
                        loadChildren: () => import('./modules/rentgen/rentgen.module').then(m => m.RentgenModule),
                        canActivate: [PermissionGuard]
                    }
                ],
                canActivate: [NoAuthGuard]
            },
            { path: 'profile', component: ProfileComponent },
            { path: 'error', component: AppErrorComponent },
            { path: 'access', component: AppAccessdeniedComponent },
            { path: 'notfound', component: AppNotfoundComponent },
            { path: 'login', component: AppLoginComponent, canActivate: [NoAuthGuard] },
            { path: 'assets/**', canActivate: [NoAuthGuard], redirectTo: '' },
            { path: '**', redirectTo: '/notfound' },
        ], { scrollPositionRestoration: 'enabled' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}

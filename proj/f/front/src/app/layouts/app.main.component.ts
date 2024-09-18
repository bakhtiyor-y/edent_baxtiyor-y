import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../core/services';

@Component({
    selector: 'app-main',
    templateUrl: './app.main.component.html',
})
export class AppMainComponent implements OnInit {

    constructor(private userService: UserService, private router: Router) {
    }
    ngOnInit(): void {
        if (this.userService.HasRole('admin')) {
            this.router.navigateByUrl('/dashboard/admin');
        } 

        if (this.userService.HasPermission('COMMON_DOCTOR')) {
            this.router.navigateByUrl('/dashboard/doctor');
        }
        else if (this.userService.HasRole('COMMON_RECEPTION')) {
            this.router.navigateByUrl('/dashboard/reception');
        }
        else if (this.userService.HasRole('COMMON_RENTGEN')) {
            this.router.navigateByUrl('/dashboard/rentgen');
        }
        else if (this.userService.HasRole('COMMON_CASHIER')) {
            this.router.navigateByUrl('/dashboard/cashier');
        }
        else if (this.userService.HasRole('COMMON_USER_MANAGEMENT')) {
            this.router.navigateByUrl('/dashboard/user-management');
        }
        else if (this.userService.HasRole('COMMON_INVENTORY_MANAGEMENT')) {
            this.router.navigateByUrl('/dashboard/inventory-management');
        }
        else if (this.userService.HasRole('COMMON_REPORTS')) {
            this.router.navigateByUrl('/dashboard/reports');
        }
        else if (this.userService.HasRole('COMMON_MANUALS')) {
            this.router.navigateByUrl('/dashboard/manuals');
        }
        else if (this.userService.HasRole('COMMON_APP_SETTINGS')) {
            this.router.navigateByUrl('/dashboard/app-settings');
        }
        else {
            this.router.navigateByUrl('/login');
        }
    }
}

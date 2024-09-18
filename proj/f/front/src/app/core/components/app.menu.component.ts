import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LayoutType } from '../enums/layout-type.enum';
import { OrganizationModel } from '../models/appsettings/organization.model';
import { ApiService, UserService } from '../services';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {

    model: any[];
    @Input() public appMain: any;

    orgInfo: OrganizationModel;

    constructor(private userService: UserService, private translate: TranslateService, private apiService: ApiService) {

    }

    ngOnInit() {
        this.model = this.getDashboardMenuModel();
        this.orgInfo = JSON.parse(localStorage.getItem('orgInfo'));
        if (!this.orgInfo) {
            this.apiService.get('api/Organization')
                .toPromise()
                .then(o => {
                    this.orgInfo = o;
                    localStorage.setItem('orgInfo', JSON.stringify(o));
                });
        }
    }

    public getDashboardMenuModel(): any[] {

        const userManagementMenu = [
            { separator: true },
            {
                label: this.translate.instant('USER_MANAGEMENT'), icon: 'pi pi-fw pi-star', routerLink: ['/dashboard/user-management/users'],
                items: [
                    { label: this.translate.instant('USERS'), icon: 'pi pi-fw pi-user', routerLink: ['/dashboard/user-management/users'] },
                    { label: this.translate.instant('ROLES'), icon: 'pi pi-fw pi-check-square', routerLink: ['/dashboard/user-management/roles'] },
                    { label: this.translate.instant('DOCTORS'), icon: 'pi pi-fw pi-users', routerLink: ['/dashboard/user-management/doctors'] },
                    { label: this.translate.instant('TECHNICS'), icon: 'pi pi-fw pi-users', routerLink: ['/dashboard/user-management/technics'] },
                    { label: this.translate.instant('PATIENTS'), icon: 'pi pi-fw pi-users', routerLink: ['/dashboard/user-management/patients'] },
                ]
            },
        ];

        const inventoryManagementMenu = [
            { separator: true },
            {
                label: this.translate.instant('INVENTORY_MANAGEMENT'), icon: 'pi pi-fw pi-compass', routerLink: ['/dashboard/inventory-management'],
                items: [
                    { label: this.translate.instant('INVENTORIES'), icon: 'pi pi-fw pi-list', routerLink: ['/dashboard/manuals/inventories'] },
                    { label: this.translate.instant('INCOME'), icon: 'pi pi-fw pi-sign-in', routerLink: ['/dashboard/inventory-management/income'] },
                    { label: this.translate.instant('OUTCOME'), icon: 'pi pi-fw pi-sign-out', routerLink: ['/dashboard/inventory-management/outcome'] }
                ]
            },
        ];

        const reportsMenu = [
            { separator: true },
            {
                label: this.translate.instant('REPORTS'), icon: 'pi pi-fw pi-align-left',
                items: [
                    { label: this.translate.instant('RECEPT_REPORT'), icon: 'pi pi-fw pi-book', routerLink: ['/dashboard/reports/recept-report'] },
                    { label: this.translate.instant('PARTNERS_REPORT'), icon: 'pi pi-fw pi-tablet', routerLink: ['/dashboard/reports/partners-report'] },
                    { label: this.translate.instant('DOCTORS_REPORT'), icon: 'pi pi-fw pi-users', routerLink: ['/dashboard/reports/doctors-report'] },
                    { label: this.translate.instant('PATIENTS_REPORT'), icon: 'pi pi-fw pi-table', routerLink: ['/dashboard/reports/patients-report'] },
                    { label: this.translate.instant('DENTAL_SERVICE_REPORT'), icon: 'pi pi-fw pi-wallet', routerLink: ['/dashboard/reports/dental-service-report'] },
                ]
            },
        ];

        const manualsMenu = [
            { separator: true },
            {
                label: this.translate.instant('MANUALS'), icon: 'pi pi-fw pi-compass', routerLink: ['/dashboard/manuals'],
                items: [
                    { label: this.translate.instant('DENTAL_SERVICES'), icon: 'pi pi-fw pi-share-alt', routerLink: ['/dashboard/manuals/dental-service'] },
                    { label: this.translate.instant('DENTAL_SERVICE_GROUPS'), icon: 'pi pi-fw pi-folder', routerLink: ['/dashboard/manuals/dental-service-group'] },
                    { label: this.translate.instant('DENTAL_SERVICE_CATEGORIES'), icon: 'pi pi-fw pi-folder-open', routerLink: ['/dashboard/manuals/dental-service-category'] },
                    { label: this.translate.instant('DENTAL_CHAIRS'), icon: 'pi pi-fw pi-folder-open', routerLink: ['/dashboard/manuals/dental-chairs'] },
                    { label: this.translate.instant('MEASUREMENT_UNITS'), icon: 'pi pi-fw pi-compass', routerLink: ['/dashboard/manuals/measurement-unit'] },
                    { label: this.translate.instant('PARTNERS'), icon: 'pi pi-fw pi-clone', routerLink: ['/dashboard/manuals/partners'] },
                    { label: this.translate.instant('PROFESSIONS'), icon: 'pi pi-fw pi-briefcase', routerLink: ['/dashboard/manuals/professions'] },
                    { label: this.translate.instant('DIAGNOSES'), icon: 'pi pi-fw pi-pencil', routerLink: ['/dashboard/manuals/diagnoses'] },
                    { label: this.translate.instant('COUNTRIES'), icon: 'pi pi-fw pi-map', routerLink: ['/dashboard/manuals/countries'] },
                    { label: this.translate.instant('REGIONS'), icon: 'pi pi-fw pi-map-marker', routerLink: ['/dashboard/manuals/regions'] },
                    { label: this.translate.instant('CITIES'), icon: 'pi pi-fw pi-directions', routerLink: ['/dashboard/manuals/cities'] },
                ]
            },
        ];

        const appsettingsMenu = [
            { separator: true },
            {
                label: this.translate.instant('APP_SETTINGS'), icon: 'pi pi-fw pi-briefcase', routerLink: ['/pages'],
                items: [
                    { label: this.translate.instant('TERMS'), icon: 'pi pi-fw pi-percentage', routerLink: ['/dashboard/app-settings/terms'] },
                    { label: this.translate.instant('RECEPT_INVENTORIES'), icon: 'pi pi-fw pi-slack', routerLink: ['/dashboard/app-settings/recept-inventories'] },
                    { label: this.translate.instant('TEETH'), icon: 'pi pi-fw pi-apple', routerLink: ['/dashboard/app-settings/teeth'] },
                    { label: this.translate.instant('ORGANIZATION'), icon: 'pi pi-fw pi-circle-off', routerLink: ['/dashboard/app-settings/organization'] },
                ]
            }
        ];

        const doctorsMenu = [
            { separator: true },
            {
                label: this.translate.instant('WORKSPACE_DOCTOR'), icon: 'pi pi-fw pi-star', routerLink: ['/doctor'],
                items: [
                    { label: this.translate.instant('APPOINTMENTS'), icon: 'pi pi-fw pi-calendar-plus', routerLink: ['/dashboard/doctor/appointment'] },
                    { label: this.translate.instant('RECEPTS'), icon: 'pi pi-fw pi-check-square', routerLink: ['/dashboard/doctor/recept'] },
                    { label: this.translate.instant('MY_PATIENTS'), icon: 'pi pi-fw pi-users', routerLink: ['/dashboard/doctor/my-patients'] },
                    { label: this.translate.instant('SCHEDULE'), icon: 'pi pi-bookmark', routerLink: ['/dashboard/doctor/schedule'] },
                    { label: this.translate.instant('REPORT'), icon: 'pi pi-exclamation-circle', routerLink: ['/dashboard/doctor/report'] },

                ]
            }
        ];

        const cashiersMenu = [
            { separator: true },
            {
                label: this.translate.instant('WORKSPACE_CASHIER'), icon: 'pi pi-fw pi-star', routerLink: ['/dashboard/cashier'],
                items: [
                    { label: this.translate.instant('INVOICES'), icon: 'pi pi-fw pi-money-bill', routerLink: ['/dashboard/cashier/invoice'] },
                    { label: this.translate.instant('PATIENTS_REPORT'), icon: 'pi pi-fw pi-table', routerLink: ['/dashboard/cashier/patients-report'] },
                ]
            }
        ];

        const receptionMenu = [
            { separator: true },
            {
                label: this.translate.instant('WORKSPACE_RECEPTION'), icon: 'pi pi-fw pi-star', routerLink: ['/dashboard/reception'],
                items: [
                    { label: this.translate.instant('SCHEDULES'), icon: 'pi pi-fw pi-globe', routerLink: ['/dashboard/reception/doctors-schedule'] },
                    { label: this.translate.instant('APPOINTMENTS'), icon: 'pi pi-fw pi-file', routerLink: ['/dashboard/reception/appointments'] },
                    { label: this.translate.instant('PATIENTS'), icon: 'pi pi-fw pi-list', routerLink: ['/dashboard/reception/patient'] },
                    { label: this.translate.instant('RENTGEN_APPOINTMENTS'), icon: 'pi pi-fw pi-id-card', routerLink: ['/dashboard/reception/rentgen/rentgen-desk'] }
                ]
            }
        ];

        const rentgenMenu: any[] = [
            {
                label: this.translate.instant('WORKSPACE_RENTGEN'), icon: 'pi pi-fw pi-star', routerLink: ['/dashboard/rentgen'],
                items: [
                    { label: this.translate.instant('RENTGEN_RECEPTS'), icon: 'pi pi-fw pi-file', routerLink: ['/dashboard/rentgen/rentgen-recepts'] },
                ]
            }
        ];

        
        if (this.userService.HasRole('admin')) {
            return [...userManagementMenu, 
                ...inventoryManagementMenu, 
                ...reportsMenu, 
                ...manualsMenu, 
                ...appsettingsMenu, 
                ...doctorsMenu, 
                ...receptionMenu, 
                ...cashiersMenu, 
                ...rentgenMenu];
        }
        let menuItems = [];
        if (this.userService.HasPermission('COMMON_USER_MANAGEMENT')) {
            menuItems = [...menuItems, ...userManagementMenu];
        }
        if (this.userService.HasPermission('COMMON_INVENTORY_MANAGEMENT')) {
            menuItems = [...menuItems, ...inventoryManagementMenu];
        }
        if (this.userService.HasPermission('COMMON_REPORTS')) {
            menuItems = [...menuItems, ...reportsMenu];
        }
        if (this.userService.HasPermission('COMMON_DOCTOR')) {
            menuItems = [...menuItems, ...doctorsMenu];
        }
        if (this.userService.HasPermission('COMMON_MANUALS')) {
            menuItems = [...menuItems, ...manualsMenu];
        }
        if (this.userService.HasPermission('COMMON_APP_SETTINGS')) {
            menuItems = [...menuItems, ...appsettingsMenu];
        }
        if (this.userService.HasPermission('COMMON_RECEPTION')) {
            menuItems = [...menuItems, ...receptionMenu];
        }
        if (this.userService.HasPermission('COMMON_CASHIER')) {
            menuItems = [...menuItems, ...cashiersMenu];
        }
        if (this.userService.HasPermission('COMMON_RENTGEN')) {
            menuItems = [...menuItems, ...rentgenMenu];
        }
        return menuItems;

    }


}

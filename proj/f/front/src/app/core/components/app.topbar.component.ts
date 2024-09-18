import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { AppComponent } from '../../app.component';
import { BreadcrumbService } from '../services/app.breadcrumb.service';
import { Input } from '@angular/core';
import { ApiService, UserService } from '../services';
import { UserModel } from '../models/user-management';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent implements OnDestroy, OnInit {

    subscription: Subscription;
    public currentUser: UserModel;
    public priceDilog: boolean;

    items: MenuItem[];

    @Input() public appMain: any;

    constructor(public breadcrumbService: BreadcrumbService,
        public app: AppComponent,
        private userService: UserService,
        private apiService: ApiService) {
        this.subscription = breadcrumbService.itemsHandler.subscribe(response => {
            this.items = response;
        });

    }

    ngOnInit(): void {
        this.currentUser = this.userService.getProfile();
        if (!this.currentUser) {
            this.apiService.get('api/User/GetProfile')
                .toPromise()
                .then(u => {
                    this.currentUser = u;
                    this.userService.setProfile(u);
                })
                .catch(error => { })
                .finally(() => { });
        }
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    public viewPrice() {
        this.priceDilog = true;
    }

    public closePrice() {
        this.priceDilog = false;
    }

    public logout() {
        this.userService.purgeAuth();
    }
}

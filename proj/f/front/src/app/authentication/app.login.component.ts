import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppComponent } from '../app.component';
import { LoginInfo, LoginModel } from '../core/models/authentication';
import { ApiService, JwtService, UserService } from '../core/services';

@Component({
    selector: 'app-login',
    templateUrl: './app.login.component.html',
})
export class AppLoginComponent implements OnInit {

    public loginModel: LoginModel;

    public loginForm = this.fb.group({
        userName: this.fb.control('', Validators.required),
        password: this.fb.control('', Validators.required)
    });

    constructor(public app: AppComponent,
        private apiService: ApiService,
        private userService: UserService,
        private router: Router,
        private fb: FormBuilder,
        private messageService: MessageService,
        private translate: TranslateService) {
    }
    ngOnInit(): void {
        this.loginModel = {} as LoginModel;
    }

    public onLogin(): void {
        if (this.loginForm.valid) {
            this.loginModel = this.loginForm.value;
            this.apiService.post('api/account/login', this.loginModel)
                .toPromise()
                .then(th => {
                    const loginInfo: LoginInfo = th;
                    this.userService.setAuth(loginInfo);
                    if (this.userService.HasRole('admin')) {
                        this.router.navigateByUrl('/dashboard/admin');
                    }
                    else if (this.userService.HasRole('doctor')) {
                        this.router.navigateByUrl('/dashboard/doctor');
                    }
                    else if (this.userService.HasRole('reception')) {
                        this.router.navigateByUrl('/dashboard/reception');
                    }
                    else if (this.userService.HasRole('rentgen')) {
                        this.router.navigateByUrl('/dashboard/rentgen');
                    }
                    else {
                        this.router.navigateByUrl('/dashboard/cashier/invoice');
                    }
                }).catch((error) => {
                    this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_LOGIN'), life: 3000 });
                }).finally(() => { });
        }
    }
}

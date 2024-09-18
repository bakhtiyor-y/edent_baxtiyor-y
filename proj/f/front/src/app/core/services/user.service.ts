import { Injectable } from '@angular/core';
import { JwtService } from './jwt.service';
import { Router } from '@angular/router';
import { LoginInfo } from '../../core/models/authentication';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserModel } from '../models/user-management';


@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(private jwtService: JwtService,
        private router: Router
    ) {

    }

    public setAuth(login: LoginInfo) {
        login.expiresAt = new Date(Date.now() + (login.expiresIn - 300) * 1000);
        this.jwtService.saveToken(login);
    }

    public purgeAuth() {
        // Remove JWT from localstorage
        this.jwtService.destroyToken();
        localStorage.removeItem('userInfo');
        this.router.navigateByUrl('/login');
    }

    public setProfile(profile: UserModel) {
        localStorage.setItem('userInfo', JSON.stringify(profile));
    }

    public getProfile() {
        const profile: UserModel = JSON.parse(localStorage.getItem('userInfo'));
        return profile;
    }


    public hasJwtToken() {
        return this.jwtService.getToken();
    }

    public getRoles() {
        const jwtToken = this.hasJwtToken();
        if (jwtToken) {
            const helper = new JwtHelperService();
            const decodedToken = helper.decodeToken(jwtToken);
            if (!(decodedToken.roles instanceof Array)) {
                return [decodedToken.roles];
            }
            return decodedToken.roles;
        }
        return [];
    }
    
    public HasRole(roleName): boolean {
        const roles = this.getRoles();
        if (roles.length > 0 && roles.find(p => p === roleName)) {
            return true;
        }
        return false;
    }

    public getPermissions() {
        const jwtToken = this.hasJwtToken();
        if (jwtToken) {
            const helper = new JwtHelperService();
            const decodedToken = helper.decodeToken(jwtToken);
            if (!(decodedToken.permission instanceof Array)) {
                return [decodedToken.permission];
            }
            return decodedToken.permission;
        }
        return [];
    }
    public HasPermission(permission): boolean {
        const permissions = this.getPermissions();
        if (permissions.length > 0 && permissions.find(p => p === permission)) {
            return true;
        }
        return false;
    }
}

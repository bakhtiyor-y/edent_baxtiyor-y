import { Injectable } from '@angular/core';
import { LoginInfo } from '../models/authentication';

@Injectable({
    providedIn: 'root'
})
export class JwtService {

    getToken(): string {
        const loginInfo: LoginInfo = this.getLoginInfo();
        if (loginInfo) {
            return loginInfo.accessToken;
        }
        return '';
    }
 
    getLoginInfo(): LoginInfo {
        const loginInfo: LoginInfo = JSON.parse(localStorage.getItem('loginInfo'));
        return loginInfo;
    }

    saveToken(login: LoginInfo) {
        localStorage.setItem('loginInfo', JSON.stringify(login));
    }

    isAccessTokenExpired() {
        const loginInfo: LoginInfo = this.getLoginInfo();
        if (!loginInfo) {
            return true;
        }
        const expiresAt = new Date(loginInfo.expiresAt);
        return expiresAt.getTime() < Date.now();
    }

    destroyToken() {
        localStorage.removeItem('loginInfo');
    }
}

import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';

@Injectable({
    providedIn: 'root'
})
export class ReceptionGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) {

    }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        // not logged in so redirect to login page with the return url
        if (!this.userService.hasJwtToken()) {
            this.router.navigateByUrl('/login');
            return false;
        }
        if (this.userService.HasRole('admin') || this.userService.HasRole('reception') || this.userService.HasRole('cashier')) {
            return true;
        }
        this.router.navigateByUrl('/access');
        return false;
    }
}

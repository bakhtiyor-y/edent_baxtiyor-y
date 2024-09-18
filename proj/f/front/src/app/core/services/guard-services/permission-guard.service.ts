import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';

@Injectable({
    providedIn: 'root'
})
export class PermissionGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | boolean {
        // not logged in so redirect to login page with the return url
        if (!this.userService.hasJwtToken()) {
            this.router.navigateByUrl('/login');
            return false;
        }
        const permissions = this.userService.getPermissions();
        if(permissions.length<=0){
            this.router.navigateByUrl('/login');
            return false; 
        }
        
        return true;
    }
}

import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { ApiService, JwtService } from 'src/app/core/services';
import { filter, switchMap, take, tap } from 'rxjs/operators';
import { LoginInfo } from '../models/authentication';
import { Router } from '@angular/router';


@Injectable()
export class HttpTokenInterceptor implements HttpInterceptor {

  private refreshTokenInProgress = false;
  private refreshTokenSubject: Subject<any> = new BehaviorSubject<any>(null);

  constructor(private jwtService: JwtService, private apiService: ApiService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if (req.url.indexOf('assets') !== -1) {
      return next.handle(req);
    }

    if (req.url.indexOf('refresh') !== -1) {
      return next.handle(req);
    }
    if (req.url.indexOf('login') !== -1) {
      return next.handle(req);
    }

    const isAccessTokenExpired = this.jwtService.isAccessTokenExpired();
    if (isAccessTokenExpired) {
      if (!this.refreshTokenInProgress) {
        this.refreshTokenInProgress = true;
        this.refreshTokenSubject.next(null);
        try {
          return this.apiService.post('api/account/refreshtoken', this.jwtService.getLoginInfo()).pipe(
            tap(x => {
            }, error => {
              this.router.navigateByUrl('/login');
              return of(null);
            }),
            switchMap((authResponse) => {
              const loginInfo: LoginInfo = authResponse;
              loginInfo.expiresAt = new Date(Date.now() + (loginInfo.expiresIn - 300) * 1000);
              this.jwtService.saveToken(loginInfo);
              this.refreshTokenInProgress = false;
              this.refreshTokenSubject.next(authResponse.refreshToken);
              return next.handle(this.injectToken(req));
            }),
          );
        } catch (error) {
          this.router.navigateByUrl('/login');
          return of(null);
        }

      } else {
        return this.refreshTokenSubject.pipe(
          filter(result => result !== null),
          take(1),
          switchMap((res) => {
            return next.handle(this.injectToken(req));
          })
        );
      }
    }
    return next.handle(this.injectToken(req));
  }

  injectToken(request: HttpRequest<any>) {
    const token = this.jwtService.getToken();
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}

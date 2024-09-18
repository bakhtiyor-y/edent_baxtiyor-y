import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    private actionUrl: string;
    constructor(
        public http: HttpClient,
    ) {
    }

    handleError(error) {
        const errorMessage: any = { status: 0, message: '', error: null };
        if (error.error instanceof ErrorEvent) {
            // client-side error
            errorMessage.message = `Error: ${error.error.message}`;
            errorMessage.status = 0;
            errorMessage.error = null;
        } else {
            // server-side error
            errorMessage.status = error.status;
            errorMessage.message = `Message: ${error.message}`;
            errorMessage.error = error.error;
        }
        return throwError(errorMessage);
    }

    get(path?: string, params?: HttpParams): Observable<any> {
        path = path || this.actionUrl;
        return this.http.get(path, { params })
            .pipe(catchError(this.handleError));

    }

    post(path: string, body: object = {}, options?: {
        headers?: HttpHeaders | {
            [header: string]: string | string[];
        };
        observe?: 'body';
        params?: HttpParams | {
            [param: string]: string | string[];
        };
        reportProgress?: boolean;
        responseType?: 'json';
        withCredentials?: boolean;
    }): Observable<any> {
        return this.http.post(
            path,
            body,
            options).pipe(catchError(this.handleError));
    }

    put(path: string, body: object = {}, options?: {
        headers?: HttpHeaders | {
            [header: string]: string | string[];
        };
        observe?: 'body';
        params?: HttpParams | {
            [param: string]: string | string[];
        };
        reportProgress?: boolean;
        responseType?: 'json';
        withCredentials?: boolean;
    }): Observable<any> {
        return this.http.put(
            path,
            body,
            options).pipe(catchError(this.handleError));
    }

    delete(path): Observable<any> {
        return this.http.delete(
            path
        ).pipe(catchError(this.handleError));
    }
}

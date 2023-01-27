import { AutorizacaoServico } from './autorizacao.service';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

@Injectable({
  providedIn: 'root',
})
export class InvalidTokenInterceptor implements HttpInterceptor {
  constructor(
    private autorizacaoServico: AutorizacaoServico,
    private snackBar: SnackResponseService
  ) { }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((errorResponse) => {
        if (errorResponse.status === 401) {
          this.autorizacaoServico.logout();
        } else {
          this.snackBar.errorHandler(errorResponse)
          // openFromComponent(SnackResponseComponent, {
          //   data: errorResponse.error,
          // });
        }
        return throwError(errorResponse);
      })
    );
  }
}

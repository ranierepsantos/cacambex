import { EMPTY, Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SnackResponseService {
  constructor(private snackBar: MatSnackBar) { }

  mostrarMensagem(msg: string, error: boolean = false): void {
    this.snackBar.open(msg, 'X', {
      duration: 8000,
      panelClass: error ? ['snackError'] : ['snackContainer'],
    });
  }
  errorHandler(e: HttpErrorResponse): Observable<any> {
    if (e) this.mostrarMensagem(e.error.mensagem, true);
    return EMPTY;
  }
}

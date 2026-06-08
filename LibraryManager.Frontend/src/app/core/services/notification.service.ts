import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  success(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2800,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['app-snackbar-success']
    });
  }

  error(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 4500,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['app-snackbar-error']
    });
  }
}

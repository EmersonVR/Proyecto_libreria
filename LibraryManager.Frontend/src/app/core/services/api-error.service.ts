import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ApiErrorService {
  getMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Unexpected error.';
    }

    if (typeof error.error === 'string' && error.error.trim()) {
      return error.error;
    }

    if (Array.isArray(error.error)) {
      return error.error
        .map(item => item?.errorMessage ?? item?.ErrorMessage ?? item?.message)
        .filter(Boolean)
        .join(' ');
    }

    if (error.error?.message) {
      return error.error.message;
    }

    if (error.status === 0) {
      return 'No se pudo conectar con la API. Revisa que el backend este corriendo en Visual Studio con el perfil https.';
    }

    return `API error ${error.status}.`;
  }
}

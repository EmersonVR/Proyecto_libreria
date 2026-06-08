import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ApiErrorService {
  getMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Ocurrio un error inesperado.';
    }

    if (typeof error.error === 'string' && error.error.trim()) {
      const businessMessage = this.getBusinessMessage(error.error);
      if (businessMessage) {
        return businessMessage;
      }

      if (error.status >= 500 || error.error.length > 220 || error.error.includes('<!DOCTYPE')) {
        return 'No se pudo completar la accion. Revisa las reglas del negocio o intenta de nuevo.';
      }

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
      return 'No se pudo conectar con la API. Revisa que el backend este corriendo en Visual Studio con IIS Express.';
    }

    return `API error ${error.status}.`;
  }

  private getBusinessMessage(rawMessage: string): string | null {
    const message = rawMessage.toLowerCase();

    if (message.includes('cannot delete a book with active loans')) {
      return 'No se puede eliminar el libro porque tiene prestamos activos.';
    }

    if (message.includes('cannot delete author with associated books')) {
      return 'No se puede eliminar el autor porque tiene libros asociados.';
    }

    if (message.includes('cannot delete category with associated books')) {
      return 'No se puede eliminar la categoria porque tiene libros asociados.';
    }

    if (message.includes('cannot delete a reader with active loans')) {
      return 'No se puede eliminar el lector porque tiene prestamos activos.';
    }

    if (message.includes('same isbn')) {
      return 'Ya existe un libro con ese ISBN.';
    }

    if (message.includes('same email')) {
      return 'Ya existe un lector con ese email.';
    }

    if (message.includes('no available copies')) {
      return 'El libro seleccionado no tiene copias disponibles.';
    }

    if (message.includes('already returned')) {
      return 'Ese prestamo ya fue devuelto.';
    }

    return null;
  }
}

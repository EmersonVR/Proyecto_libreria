import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { Loan, LoanCreate } from './loan.model';

@Injectable({ providedIn: 'root' })
export class LoansService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/loans`;

  getAll() {
    return this.http.get<Loan[]>(this.url);
  }

  getActive() {
    return this.http.get<Loan[]>(`${this.url}/active`);
  }

  getByReader(readerId: number) {
    return this.http.get<Loan[]>(`${this.url}/by-reader/${readerId}`);
  }

  create(dto: LoanCreate) {
    return this.http.post<Loan>(this.url, dto);
  }

  returnLoan(id: number) {
    return this.http.put<void>(`${this.url}/${id}/return`, {});
  }
}

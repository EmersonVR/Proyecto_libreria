import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { ExternalBooksResponse, ExternalQuote } from './external-content.model';

@Injectable({ providedIn: 'root' })
export class ExternalContentService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/external-content`;

  getBooks(practiceSource?: string) {
    const headers = practiceSource?.trim()
      ? new HttpHeaders({ 'X-Practice-Source': practiceSource.trim() })
      : undefined;

    return this.http.get<ExternalBooksResponse>(`${this.url}/books`, { headers });
  }

  getQuote() {
    return this.http.get<ExternalQuote>(`${this.url}/quote`);
  }
}

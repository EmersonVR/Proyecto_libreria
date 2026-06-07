import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { Reader, ReaderCreate, ReaderUpdate } from './reader.model';

@Injectable({ providedIn: 'root' })
export class ReadersService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/readers`;

  getAll() {
    return this.http.get<Reader[]>(this.url);
  }

  create(dto: ReaderCreate) {
    return this.http.post<Reader>(this.url, dto);
  }

  update(id: number, dto: ReaderUpdate) {
    return this.http.put<void>(`${this.url}/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { Author, AuthorCreate, AuthorUpdate } from './author.model';

@Injectable({ providedIn: 'root' })
export class AuthorsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/authors`;

  getAll() {
    return this.http.get<Author[]>(this.url);
  }

  create(dto: AuthorCreate) {
    return this.http.post<Author>(this.url, dto);
  }

  update(id: number, dto: AuthorUpdate) {
    return this.http.put<void>(`${this.url}/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

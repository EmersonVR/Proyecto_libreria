import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { Book, BookCreate, BookUpdate } from './book.model';

@Injectable({ providedIn: 'root' })
export class BooksService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/books`;

  getAll() {
    return this.http.get<Book[]>(this.url);
  }

  searchByTitle(title: string) {
    return this.http.get<Book[]>(`${this.url}/search`, {
      params: new HttpParams().set('title', title)
    });
  }

  getByAuthor(authorId: number) {
    return this.http.get<Book[]>(`${this.url}/by-author/${authorId}`);
  }

  getByCategory(categoryId: number) {
    return this.http.get<Book[]>(`${this.url}/by-category/${categoryId}`);
  }

  getAvailable() {
    return this.http.get<Book[]>(`${this.url}/available`);
  }

  create(dto: BookCreate) {
    return this.http.post<Book>(this.url, dto);
  }

  update(id: number, dto: BookUpdate) {
    return this.http.put<void>(`${this.url}/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

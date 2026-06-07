import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../../core/config/api.config';
import { Category, CategoryCreate, CategoryUpdate } from './category.model';

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/categories`;

  getAll() {
    return this.http.get<Category[]>(this.url);
  }

  create(dto: CategoryCreate) {
    return this.http.post<Category>(this.url, dto);
  }

  update(id: number, dto: CategoryUpdate) {
    return this.http.put<void>(`${this.url}/${id}`, dto);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

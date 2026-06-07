export interface Category {
  categoryId: number;
  name: string;
  description?: string | null;
}

export interface CategoryCreate {
  name: string;
  description?: string | null;
}

export interface CategoryUpdate extends CategoryCreate {
  categoryId: number;
}

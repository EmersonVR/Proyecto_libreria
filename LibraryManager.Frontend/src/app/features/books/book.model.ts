export interface Book {
  bookId: number;
  title: string;
  isbn: string;
  publicationYear: number;
  availableCopies: number;
  authorId: number;
  authorName?: string | null;
  categoryId: number;
  categoryName?: string | null;
}

export interface BookCreate {
  title: string;
  isbn: string;
  publicationYear: number;
  availableCopies: number;
  authorId: number;
  categoryId: number;
}

export interface BookUpdate extends BookCreate {
  bookId: number;
}

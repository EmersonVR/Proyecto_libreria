export interface Author {
  authorId: number;
  name: string;
  birthDate?: string | null;
}

export interface AuthorCreate {
  name: string;
  birthDate?: string | null;
}

export interface AuthorUpdate extends AuthorCreate {
  authorId: number;
}

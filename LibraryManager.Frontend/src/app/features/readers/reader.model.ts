export interface Reader {
  readerId: number;
  name: string;
  email: string;
  phone?: string | null;
}

export interface ReaderCreate {
  name: string;
  email: string;
  phone?: string | null;
}

export interface ReaderUpdate extends ReaderCreate {
  readerId: number;
}

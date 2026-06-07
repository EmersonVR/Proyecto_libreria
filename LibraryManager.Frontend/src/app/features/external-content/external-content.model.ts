export interface ExternalBook {
  title?: string | null;
  authors: string[];
  firstPublishYear?: number | null;
  isbn?: string | null;
}

export interface ExternalBooksResponse {
  practiceSource?: string | null;
  result: ExternalBook[];
}

export interface ExternalQuote {
  id: number;
  quote?: string | null;
  author?: string | null;
}

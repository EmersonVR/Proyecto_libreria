export interface Loan {
  loanId: number;
  bookId: number;
  bookTitle?: string | null;
  readerId: number;
  readerName?: string | null;
  loanDate: string;
  returnDate?: string | null;
  status: string;
}

export interface LoanCreate {
  bookId: number;
  readerId: number;
}

import { DatePipe } from '@angular/common';
import { Component, OnInit, QueryList, ViewChildren, inject } from '@angular/core';
import { FormBuilder, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { forkJoin } from 'rxjs';
import { ApiErrorService } from '../../core/services/api-error.service';
import { NotificationService } from '../../core/services/notification.service';
import { Book } from '../books/book.model';
import { BooksService } from '../books/books.service';
import { Reader } from '../readers/reader.model';
import { ReadersService } from '../readers/readers.service';
import { Loan } from './loan.model';
import { LoansService } from './loans.service';

@Component({
  selector: 'app-loans-page',
  imports: [
    DatePipe,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSnackBarModule,
    MatTableModule
  ],
  templateUrl: './loans-page.component.html'
})
export class LoansPageComponent implements OnInit {
  @ViewChildren(FormGroupDirective) private formDirectives?: QueryList<FormGroupDirective>;
  private readonly fb = inject(FormBuilder);
  private readonly loansService = inject(LoansService);
  private readonly booksService = inject(BooksService);
  private readonly readersService = inject(ReadersService);
  private readonly notify = inject(NotificationService);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['bookTitle', 'readerName', 'loanDate', 'returnDate', 'status', 'actions'];
  loans: Loan[] = [];
  books: Book[] = [];
  readers: Reader[] = [];
  loading = false;

  readonly form = this.fb.nonNullable.group({
    bookId: [0, [Validators.required, Validators.min(1)]],
    readerId: [0, [Validators.required, Validators.min(1)]]
  });

  readonly filters = this.fb.nonNullable.group({
    readerId: [0],
    activeOnly: [false]
  });

  ngOnInit() {
    this.loadInitialData();
  }

  loadInitialData() {
    this.loading = true;
    forkJoin({
      loans: this.loansService.getAll(),
      books: this.booksService.getAvailable(),
      readers: this.readersService.getAll()
    }).subscribe({
      next: result => {
        this.loans = result.loans;
        this.books = result.books;
        this.readers = result.readers;
        this.loading = false;
      },
      error: error => this.showError(error)
    });
  }

  applyFilters() {
    const readerId = this.filters.controls.readerId.value;
    const activeOnly = this.filters.controls.activeOnly.value;
    this.loading = true;

    const request = readerId
      ? this.loansService.getByReader(readerId)
      : activeOnly
        ? this.loansService.getActive()
        : this.loansService.getAll();

    request.subscribe({
      next: loans => {
        this.loans = loans;
        this.loading = false;
      },
      error: error => this.showError(error)
    });
  }

  clearFilters() {
    this.filters.reset({ readerId: 0, activeOnly: false });
    this.applyFilters();
  }

  createLoan() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loansService.create(this.form.getRawValue()).subscribe({
      next: () => {
        this.notify.success('Prestamo creado correctamente.');
        this.resetForm();
        this.loadInitialData();
      },
      error: error => this.showError(error)
    });
  }

  returnLoan(loan: Loan) {
    this.loansService.returnLoan(loan.loanId).subscribe({
      next: () => {
        this.notify.success('Prestamo devuelto correctamente.');
        this.loadInitialData();
      },
      error: error => this.showError(error)
    });
  }

  private resetForm() {
    const defaults = { bookId: 0, readerId: 0 };
    this.formDirectives?.last?.resetForm(defaults);
    this.form.reset(defaults);
  }

  private showError(error: unknown) {
    this.loading = false;
    this.notify.error(this.errors.getMessage(error));
  }
}


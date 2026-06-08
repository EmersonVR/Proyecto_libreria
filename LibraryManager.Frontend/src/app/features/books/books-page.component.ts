import { Component, OnInit, QueryList, ViewChildren, inject } from '@angular/core';
import { FormBuilder, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { forkJoin, Observable } from 'rxjs';
import { ApiErrorService } from '../../core/services/api-error.service';
import { NotificationService } from '../../core/services/notification.service';
import { Author } from '../authors/author.model';
import { AuthorsService } from '../authors/authors.service';
import { Category } from '../categories/category.model';
import { CategoriesService } from '../categories/categories.service';
import { DigitsOnlyDirective } from '../../shared/digits-only.directive';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { Book } from './book.model';
import { BooksService } from './books.service';

@Component({
  selector: 'app-books-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSnackBarModule,
    MatTableModule,
    DigitsOnlyDirective
  ],
  templateUrl: './books-page.component.html'
})
export class BooksPageComponent implements OnInit {
  @ViewChildren(FormGroupDirective) private formDirectives?: QueryList<FormGroupDirective>;
  private readonly fb = inject(FormBuilder);
  private readonly booksService = inject(BooksService);
  private readonly authorsService = inject(AuthorsService);
  private readonly categoriesService = inject(CategoriesService);
  private readonly notify = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['title', 'isbn', 'authorName', 'categoryName', 'availableCopies', 'actions'];
  books: Book[] = [];
  authors: Author[] = [];
  categories: Category[] = [];
  loading = false;
  editing: Book | null = null;

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    isbn: ['', [Validators.required, Validators.maxLength(30)]],
    publicationYear: [new Date().getFullYear(), [Validators.required, Validators.min(1000)]],
    availableCopies: [1, [Validators.required, Validators.min(0)]],
    authorId: [0, [Validators.required, Validators.min(1)]],
    categoryId: [0, [Validators.required, Validators.min(1)]]
  });

  readonly filters = this.fb.nonNullable.group({
    title: [''],
    authorId: [0],
    categoryId: [0],
    availableOnly: [false]
  });

  ngOnInit() {
    this.loading = true;
    forkJoin({
      books: this.booksService.getAll(),
      authors: this.authorsService.getAll(),
      categories: this.categoriesService.getAll()
    }).subscribe({
      next: result => {
        this.books = result.books;
        this.authors = result.authors;
        this.categories = result.categories;
        this.loading = false;
      },
      error: error => this.showError(error)
    });
  }

  applyFilters() {
    const title = this.filters.controls.title.value.trim();
    const authorId = this.filters.controls.authorId.value;
    const categoryId = this.filters.controls.categoryId.value;
    const availableOnly = this.filters.controls.availableOnly.value;

    this.loading = true;
    const request = title
      ? this.booksService.searchByTitle(title)
      : authorId
        ? this.booksService.getByAuthor(authorId)
        : categoryId
          ? this.booksService.getByCategory(categoryId)
          : availableOnly
            ? this.booksService.getAvailable()
            : this.booksService.getAll();

    request.subscribe({
      next: books => {
        this.books = books;
        this.loading = false;
      },
      error: error => this.showError(error)
    });
  }

  clearFilters() {
    this.filters.reset({ title: '', authorId: 0, categoryId: 0, availableOnly: false });
    this.applyFilters();
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto = {
      title: this.form.controls.title.value.trim(),
      isbn: this.form.controls.isbn.value.trim(),
      publicationYear: Number(this.form.controls.publicationYear.value),
      availableCopies: Number(this.form.controls.availableCopies.value),
      authorId: this.form.controls.authorId.value,
      categoryId: this.form.controls.categoryId.value
    };
    const request: Observable<unknown> = this.editing
      ? this.booksService.update(this.editing.bookId, { bookId: this.editing.bookId, ...dto })
      : this.booksService.create(dto);

    request.subscribe({
      next: () => {
        this.notify.success('Libro guardado correctamente.');
        this.reset();
        this.applyFilters();
      },
      error: (error: unknown) => this.showError(error)
    });
  }

  edit(book: Book) {
    this.editing = book;
    this.form.patchValue({
      title: book.title,
      isbn: book.isbn,
      publicationYear: book.publicationYear,
      availableCopies: book.availableCopies,
      authorId: book.authorId,
      categoryId: book.categoryId
    });
  }


  confirmDelete(book: Book) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete book',
        message: `Delete ${book.title}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.booksService.delete(book.bookId).subscribe({
        next: () => {
          this.notify.success('Libro eliminado correctamente.');
          this.applyFilters();
        },
        error: error => this.showError(error)
      });
    });
  }

  reset() {
    const defaults = {
      title: '',
      isbn: '',
      publicationYear: new Date().getFullYear(),
      availableCopies: 1,
      authorId: 0,
      categoryId: 0
    };
    this.editing = null;
    this.formDirectives?.last?.resetForm(defaults);
    this.form.reset(defaults);
  }

  private showError(error: unknown) {
    this.loading = false;
    this.notify.error(this.errors.getMessage(error));
  }
}




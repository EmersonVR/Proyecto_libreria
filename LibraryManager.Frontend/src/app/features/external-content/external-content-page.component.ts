import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ApiErrorService } from '../../core/services/api-error.service';
import { ExternalBook, ExternalQuote } from './external-content.model';
import { ExternalContentService } from './external-content.service';

@Component({
  selector: 'app-external-content-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './external-content-page.component.html',
  styleUrl: './external-content-page.component.scss'
})
export class ExternalContentPageComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(ExternalContentService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly errors = inject(ApiErrorService);

  books: ExternalBook[] = [];
  quote: ExternalQuote | null = null;
  practiceSource?: string | null;
  loadingBooks = false;
  loadingQuote = false;

  readonly form = this.fb.nonNullable.group({
    practiceSource: ['AngularFrontend']
  });

  ngOnInit() {
    this.loadBooks();
    this.loadQuote();
  }

  loadBooks() {
    this.loadingBooks = true;
    this.service.getBooks(this.form.controls.practiceSource.value).subscribe({
      next: response => {
        this.books = response.result;
        this.practiceSource = response.practiceSource;
        this.loadingBooks = false;
      },
      error: error => this.showError(error, 'books')
    });
  }

  loadQuote() {
    this.loadingQuote = true;
    this.service.getQuote().subscribe({
      next: quote => {
        this.quote = quote;
        this.loadingQuote = false;
      },
      error: error => this.showError(error, 'quote')
    });
  }

  private showError(error: unknown, section: 'books' | 'quote') {
    if (section === 'books') {
      this.loadingBooks = false;
    } else {
      this.loadingQuote = false;
    }

    this.snackBar.open(this.errors.getMessage(error), 'Close', { duration: 4500 });
  }
}

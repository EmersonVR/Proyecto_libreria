import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { Observable } from 'rxjs';
import { ApiErrorService } from '../../core/services/api-error.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { Author } from './author.model';
import { AuthorsService } from './authors.service';

@Component({
  selector: 'app-authors-page',
  imports: [
    DatePipe,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTableModule
  ],
  templateUrl: './authors-page.component.html'
})
export class AuthorsPageComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(AuthorsService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['name', 'birthDate', 'actions'];
  authors: Author[] = [];
  loading = false;
  editing: Author | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    birthDate: ['']
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: authors => {
        this.authors = authors;
        this.loading = false;
      },
      error: error => this.showError(error)
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const dto = {
      name: this.form.controls.name.value.trim(),
      birthDate: this.form.controls.birthDate.value || null
    };

    const request: Observable<unknown> = this.editing
      ? this.service.update(this.editing.authorId, { authorId: this.editing.authorId, ...dto })
      : this.service.create(dto);

    request.subscribe({
      next: () => {
        this.snackBar.open('Author saved.', 'Close', { duration: 2500 });
        this.reset();
        this.load();
      },
      error: (error: unknown) => this.showError(error)
    });
  }

  edit(author: Author) {
    this.editing = author;
    this.form.patchValue({
      name: author.name,
      birthDate: author.birthDate ? author.birthDate.substring(0, 10) : ''
    });
  }

  confirmDelete(author: Author) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete author',
        message: `Delete ${author.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.service.delete(author.authorId).subscribe({
        next: () => {
          this.snackBar.open('Author deleted.', 'Close', { duration: 2500 });
          this.load();
        },
        error: error => this.showError(error)
      });
    });
  }

  reset() {
    this.editing = null;
    this.form.reset({ name: '', birthDate: '' });
  }

  private showError(error: unknown) {
    this.loading = false;
    this.snackBar.open(this.errors.getMessage(error), 'Close', { duration: 4500 });
  }
}

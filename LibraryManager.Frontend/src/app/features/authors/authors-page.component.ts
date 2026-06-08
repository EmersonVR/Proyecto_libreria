import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { Observable } from 'rxjs';
import { ApiErrorService } from '../../core/services/api-error.service';
import { NotificationService } from '../../core/services/notification.service';
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
    MatDatepickerModule,
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
  @ViewChild(FormGroupDirective) private formDirective?: FormGroupDirective;
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(AuthorsService);
  private readonly notify = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['name', 'birthDate', 'actions'];
  authors: Author[] = [];
  loading = false;
  editing: Author | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    birthDate: [null as Date | null]
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
      birthDate: this.toDateOnly(this.form.controls.birthDate.value)
    };

    const request: Observable<unknown> = this.editing
      ? this.service.update(this.editing.authorId, { authorId: this.editing.authorId, ...dto })
      : this.service.create(dto);

    request.subscribe({
      next: () => {
        this.notify.success('Autor guardado correctamente.');
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
      birthDate: author.birthDate ? new Date(author.birthDate) : null
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
          this.notify.success('Autor eliminado correctamente.');
          this.load();
        },
        error: error => this.showError(error)
      });
    });
  }

  reset() {
    const defaults = { name: '', birthDate: null as Date | null };
    this.editing = null;
    this.formDirective?.resetForm(defaults);
    this.form.reset(defaults);
  }

  private toDateOnly(value: Date | null): string | null {
    if (!value) {
      return null;
    }

    const year = value.getFullYear();
    const month = String(value.getMonth() + 1).padStart(2, '0');
    const day = String(value.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private showError(error: unknown) {
    this.loading = false;
    this.notify.error(this.errors.getMessage(error));
  }
}




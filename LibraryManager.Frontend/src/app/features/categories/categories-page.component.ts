import { Component, OnInit, inject } from '@angular/core';
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
import { Category } from './category.model';
import { CategoriesService } from './categories.service';

@Component({
  selector: 'app-categories-page',
  imports: [
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
  templateUrl: './categories-page.component.html'
})
export class CategoriesPageComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(CategoriesService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['name', 'description', 'actions'];
  categories: Category[] = [];
  loading = false;
  editing: Category | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(300)]]
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: categories => {
        this.categories = categories;
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
      description: this.form.controls.description.value.trim() || null
    };
    const request: Observable<unknown> = this.editing
      ? this.service.update(this.editing.categoryId, { categoryId: this.editing.categoryId, ...dto })
      : this.service.create(dto);

    request.subscribe({
      next: () => {
        this.snackBar.open('Category saved.', 'Close', { duration: 2500 });
        this.reset();
        this.load();
      },
      error: (error: unknown) => this.showError(error)
    });
  }

  edit(category: Category) {
    this.editing = category;
    this.form.patchValue({
      name: category.name,
      description: category.description ?? ''
    });
  }

  confirmDelete(category: Category) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete category',
        message: `Delete ${category.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.service.delete(category.categoryId).subscribe({
        next: () => {
          this.snackBar.open('Category deleted.', 'Close', { duration: 2500 });
          this.load();
        },
        error: error => this.showError(error)
      });
    });
  }

  reset() {
    this.editing = null;
    this.form.reset({ name: '', description: '' });
  }

  private showError(error: unknown) {
    this.loading = false;
    this.snackBar.open(this.errors.getMessage(error), 'Close', { duration: 4500 });
  }
}

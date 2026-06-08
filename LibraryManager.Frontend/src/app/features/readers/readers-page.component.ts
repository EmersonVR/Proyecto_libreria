import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
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
import { DigitsOnlyDirective } from '../../shared/digits-only.directive';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { Reader } from './reader.model';
import { ReadersService } from './readers.service';

@Component({
  selector: 'app-readers-page',
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
    MatTableModule,
    DigitsOnlyDirective
  ],
  templateUrl: './readers-page.component.html'
})
export class ReadersPageComponent implements OnInit {
  @ViewChild(FormGroupDirective) private formDirective?: FormGroupDirective;
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(ReadersService);
  private readonly notify = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly errors = inject(ApiErrorService);

  readonly displayedColumns = ['name', 'email', 'phone', 'actions'];
  readers: Reader[] = [];
  loading = false;
  editing: Reader | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
    phone: ['', [Validators.maxLength(30)]]
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: readers => {
        this.readers = readers;
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
      email: this.form.controls.email.value.trim().toLowerCase(),
      phone: this.form.controls.phone.value.trim() || null
    };
    const request: Observable<unknown> = this.editing
      ? this.service.update(this.editing.readerId, { readerId: this.editing.readerId, ...dto })
      : this.service.create(dto);

    request.subscribe({
      next: () => {
        this.notify.success('Lector guardado correctamente.');
        this.reset();
        this.load();
      },
      error: (error: unknown) => this.showError(error)
    });
  }

  edit(reader: Reader) {
    this.editing = reader;
    this.form.patchValue({
      name: reader.name,
      email: reader.email,
      phone: reader.phone ?? ''
    });
  }

  confirmDelete(reader: Reader) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete reader',
        message: `Delete ${reader.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.service.delete(reader.readerId).subscribe({
        next: () => {
          this.notify.success('Lector eliminado correctamente.');
          this.load();
        },
        error: error => this.showError(error)
      });
    });
  }

  reset() {
    const defaults = { name: '', email: '', phone: '' };
    this.editing = null;
    this.formDirective?.resetForm(defaults);
    this.form.reset(defaults);
  }

  private showError(error: unknown) {
    this.loading = false;
    this.notify.error(this.errors.getMessage(error));
  }
}


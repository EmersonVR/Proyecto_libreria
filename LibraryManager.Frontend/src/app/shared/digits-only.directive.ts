import { Directive, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appDigitsOnly]',
  standalone: true
})
export class DigitsOnlyDirective {
  constructor(private readonly ngControl: NgControl) {}

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    const allowedKeys = ['Backspace', 'Delete', 'Tab', 'Escape', 'Enter', 'ArrowLeft', 'ArrowRight', 'Home', 'End'];

    if (allowedKeys.includes(event.key) || event.ctrlKey || event.metaKey) {
      return;
    }

    if (!/^\d$/.test(event.key)) {
      event.preventDefault();
    }
  }

  @HostListener('input', ['$event'])
  onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const sanitizedValue = input.value.replace(/\D/g, '');

    if (input.value !== sanitizedValue) {
      input.value = sanitizedValue;
    }

    this.ngControl.control?.setValue(sanitizedValue, { emitEvent: false });
  }
}

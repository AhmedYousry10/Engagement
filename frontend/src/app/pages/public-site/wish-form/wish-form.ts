import { Component, Input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-wish-form',
  imports: [FormsModule],
  templateUrl: './wish-form.html',
  styleUrl: './wish-form.css'
})
export class WishForm {
  @Input() whatsAppNumber = '';

  guestName = '';
  guestMsg = '';
  readonly noNumberConfigured = signal(false);

  send(): void {
    const msg = this.guestMsg.trim();
    if (!msg) {
      return;
    }

    const digitsOnly = (this.whatsAppNumber || '').replace(/[^0-9]/g, '');
    if (!digitsOnly) {
      this.noNumberConfigured.set(true);
      return;
    }

    const name = this.guestName.trim();
    const text = `${name ? name + ': ' : ''}${msg}`;
    const waLink = `https://wa.me/${digitsOnly}?text=${encodeURIComponent(text)}`;
    window.open(waLink, '_blank', 'noopener');
  }
}

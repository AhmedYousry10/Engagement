import { NgStyle } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SiteContent } from '../../../core/models/site-content.model';
import { AuthService } from '../../../core/services/auth.service';
import { SiteContentService } from '../../../core/services/site-content.service';

@Component({
  selector: 'app-dashboard-settings',
  imports: [FormsModule, NgStyle],
  templateUrl: './dashboard-settings.html',
  styleUrl: './dashboard-settings.css'
})
export class DashboardSettings implements OnInit {
  content: SiteContent | null = null;
  readonly saving = signal(false);
  readonly saved = signal(false);

  currentPassword = '';
  newPassword = '';
  readonly changingPassword = signal(false);
  readonly passwordError = signal('');
  readonly passwordSaved = signal(false);

  constructor(
    private siteContentService: SiteContentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.siteContentService.get().subscribe((content) => (this.content = content));
  }

  save(): void {
    if (!this.content) {
      return;
    }

    this.saving.set(true);
    this.saved.set(false);

    this.siteContentService.update(this.content).subscribe({
      next: (updated) => {
        this.content = updated;
        this.saving.set(false);
        this.saved.set(true);
        setTimeout(() => this.saved.set(false), 2000);
      },
      error: () => this.saving.set(false)
    });
  }

  changePassword(): void {
    if (!this.currentPassword || !this.newPassword) {
      return;
    }

    this.changingPassword.set(true);
    this.passwordError.set('');
    this.passwordSaved.set(false);

    this.authService
      .changePassword({ currentPassword: this.currentPassword, newPassword: this.newPassword })
      .subscribe({
        next: () => {
          this.changingPassword.set(false);
          this.passwordSaved.set(true);
          this.currentPassword = '';
          this.newPassword = '';
          setTimeout(() => this.passwordSaved.set(false), 2000);
        },
        error: () => {
          this.changingPassword.set(false);
          this.passwordError.set('Could not change password — check your current password.');
        }
      });
  }
}

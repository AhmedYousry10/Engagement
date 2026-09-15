import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SiteContent } from '../../../core/models/site-content.model';
import { SiteContentService } from '../../../core/services/site-content.service';

@Component({
  selector: 'app-dashboard-basics',
  imports: [FormsModule],
  templateUrl: './dashboard-basics.html',
  styleUrl: './dashboard-basics.css'
})
export class DashboardBasics implements OnInit {
  content: SiteContent | null = null;
  readonly saving = signal(false);
  readonly saved = signal(false);

  constructor(private siteContentService: SiteContentService) {}

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
}

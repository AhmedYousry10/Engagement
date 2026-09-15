import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';
import { DetailCard } from '../../../core/models/detail-card.model';
import { SiteContent } from '../../../core/models/site-content.model';
import { DetailsService } from '../../../core/services/details.service';
import { SiteContentService } from '../../../core/services/site-content.service';

interface EditableCard {
  id?: number;
  title: string;
  body: string;
}

@Component({
  selector: 'app-dashboard-details',
  imports: [FormsModule],
  templateUrl: './dashboard-details.html',
  styleUrl: './dashboard-details.css'
})
export class DashboardDetails implements OnInit {
  cards: EditableCard[] = [];
  content: SiteContent | null = null;
  readonly saving = signal(false);
  readonly saved = signal(false);

  constructor(
    private detailsService: DetailsService,
    private siteContentService: SiteContentService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  private load(): void {
    this.detailsService.getAll().subscribe((cards) => {
      this.cards = cards.map((c) => ({ id: c.id, title: c.title, body: c.body }));
    });
    this.siteContentService.get().subscribe((content) => (this.content = content));
  }

  addCard(): void {
    this.cards.push({ title: '', body: '' });
  }

  removeCard(index: number): void {
    const card = this.cards[index];
    if (card.id) {
      this.detailsService.delete(card.id).subscribe(() => this.cards.splice(index, 1));
    } else {
      this.cards.splice(index, 1);
    }
  }

  save(): void {
    if (!this.content) {
      return;
    }

    this.saving.set(true);
    this.saved.set(false);

    const cardRequests: Observable<DetailCard>[] = this.cards.map((card, index) => {
      const payload = { title: card.title, body: card.body, sortOrder: index };
      return card.id ? this.detailsService.update(card.id, payload) : this.detailsService.create(payload);
    });

    forkJoin([this.siteContentService.update(this.content), ...cardRequests]).subscribe({
      next: () => {
        this.load();
        this.saving.set(false);
        this.saved.set(true);
        setTimeout(() => this.saved.set(false), 2000);
      },
      error: () => this.saving.set(false)
    });
  }
}

import { NgStyle } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RevealOnScrollDirective } from '../../../core/directives/reveal-on-scroll.directive';
import { DetailCard } from '../../../core/models/detail-card.model';
import { Photo } from '../../../core/models/photo.model';
import { SiteContent } from '../../../core/models/site-content.model';
import { DetailsService } from '../../../core/services/details.service';
import { PhotosService } from '../../../core/services/photos.service';
import { SiteContentService } from '../../../core/services/site-content.service';
import { Countdown } from '../countdown/countdown';
import { WishForm } from '../wish-form/wish-form';

@Component({
  selector: 'app-public-site',
  imports: [RouterLink, NgStyle, RevealOnScrollDirective, Countdown, WishForm],
  templateUrl: './public-site.html',
  styleUrl: './public-site.css'
})
export class PublicSite implements OnInit {
  readonly content = signal<SiteContent | null>(null);
  readonly details = signal<DetailCard[]>([]);
  readonly photos = signal<Photo[]>([]);
  readonly loading = signal(true);

  constructor(
    private siteContentService: SiteContentService,
    private detailsService: DetailsService,
    private photosService: PhotosService
  ) {}

  ngOnInit(): void {
    this.siteContentService.get().subscribe((content) => this.content.set(content));
    this.detailsService.getAll().subscribe((details) => this.details.set(details));
    this.photosService.getAll().subscribe((photos) => {
      this.photos.set(photos);
      this.loading.set(false);
    });
  }

  hasLocation(content: SiteContent): boolean {
    return !!(content.locationName || content.locationAddress || content.locationMapUrl);
  }

  photoUrl(path: string): string {
    return this.photosService.resolveUrl(path);
  }

  themeVars(content: SiteContent): Record<string, string> {
    return {
      '--rose-deep': content.colorPrimary,
      '--sage': content.colorSecondary,
      '--ivory': content.colorBackground,
      '--ink': content.colorText
    };
  }
}

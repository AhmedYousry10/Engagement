import { Component, OnInit, signal } from '@angular/core';
import { Photo } from '../../../core/models/photo.model';
import { PhotosService } from '../../../core/services/photos.service';

@Component({
  selector: 'app-dashboard-photos',
  imports: [],
  templateUrl: './dashboard-photos.html',
  styleUrl: './dashboard-photos.css'
})
export class DashboardPhotos implements OnInit {
  readonly photos = signal<Photo[]>([]);
  readonly uploading = signal(false);
  readonly error = signal('');

  constructor(private photosService: PhotosService) {}

  ngOnInit(): void {
    this.load();
  }

  private load(): void {
    this.photosService.getAll().subscribe((photos) => this.photos.set(photos));
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) {
      return;
    }

    this.uploading.set(true);
    this.error.set('');

    this.photosService.upload(file).subscribe({
      next: () => {
        this.uploading.set(false);
        input.value = '';
        this.load();
      },
      error: () => {
        this.uploading.set(false);
        input.value = '';
        this.error.set('Upload failed — please try a different image.');
      }
    });
  }

  remove(id: number): void {
    this.photosService.delete(id).subscribe(() => {
      this.photos.update((photos) => photos.filter((p) => p.id !== id));
    });
  }

  photoUrl(path: string): string {
    return this.photosService.resolveUrl(path);
  }
}

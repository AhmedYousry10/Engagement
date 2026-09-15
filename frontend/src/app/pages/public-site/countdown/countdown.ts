import { Component, Input, OnChanges, OnDestroy, signal } from '@angular/core';

interface CountdownParts {
  days: number;
  hours: number;
  minutes: number;
}

@Component({
  selector: 'app-countdown',
  imports: [],
  templateUrl: './countdown.html',
  styleUrl: './countdown.css'
})
export class Countdown implements OnChanges, OnDestroy {
  @Input({ required: true }) targetIsoDate = '';

  readonly parts = signal<CountdownParts>({ days: 0, hours: 0, minutes: 0 });

  private intervalId?: ReturnType<typeof setInterval>;

  ngOnChanges(): void {
    this.tick();
    clearInterval(this.intervalId);
    this.intervalId = setInterval(() => this.tick(), 60000);
  }

  ngOnDestroy(): void {
    clearInterval(this.intervalId);
  }

  private tick(): void {
    const target = new Date(`${this.targetIsoDate}T00:00:00`);
    const diff = Math.max(0, target.getTime() - Date.now());

    this.parts.set({
      days: Math.floor(diff / (1000 * 60 * 60 * 24)),
      hours: Math.floor((diff / (1000 * 60 * 60)) % 24),
      minutes: Math.floor((diff / (1000 * 60)) % 60)
    });
  }
}

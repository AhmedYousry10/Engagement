import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DetailCard, DetailCardUpsert } from '../models/detail-card.model';

@Injectable({ providedIn: 'root' })
export class DetailsService {
  private readonly baseUrl = `${environment.apiUrl}/details`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<DetailCard[]> {
    return this.http.get<DetailCard[]>(this.baseUrl);
  }

  create(detail: DetailCardUpsert): Observable<DetailCard> {
    return this.http.post<DetailCard>(this.baseUrl, detail);
  }

  update(id: number, detail: DetailCardUpsert): Observable<DetailCard> {
    return this.http.put<DetailCard>(`${this.baseUrl}/${id}`, detail);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

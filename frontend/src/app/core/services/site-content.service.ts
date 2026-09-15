import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SiteContent } from '../models/site-content.model';

@Injectable({ providedIn: 'root' })
export class SiteContentService {
  private readonly baseUrl = `${environment.apiUrl}/site-content`;

  constructor(private http: HttpClient) {}

  get(): Observable<SiteContent> {
    return this.http.get<SiteContent>(this.baseUrl);
  }

  update(content: SiteContent): Observable<SiteContent> {
    return this.http.put<SiteContent>(this.baseUrl, content);
  }
}

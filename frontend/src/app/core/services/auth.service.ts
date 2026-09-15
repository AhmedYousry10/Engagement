import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

interface LoginResponse {
  token: string;
  expiresAt: string;
  username: string;
}

interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

const TOKEN_KEY = 'engagement.admin.token';
const EXPIRES_KEY = 'engagement.admin.tokenExpiresAt';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly isAuthenticated = signal(this.hasValidToken());
  readonly isAuthenticated$ = this.isAuthenticated.asReadonly();

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/auth/login`, { username, password })
      .pipe(
        tap((res) => {
          localStorage.setItem(TOKEN_KEY, res.token);
          localStorage.setItem(EXPIRES_KEY, res.expiresAt);
          this.isAuthenticated.set(true);
        })
      );
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/auth/change-password`, request);
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EXPIRES_KEY);
    this.isAuthenticated.set(false);
  }

  getToken(): string | null {
    return this.hasValidToken() ? localStorage.getItem(TOKEN_KEY) : null;
  }

  isLoggedIn(): boolean {
    return this.hasValidToken();
  }

  private hasValidToken(): boolean {
    const token = localStorage.getItem(TOKEN_KEY);
    const expiresAt = localStorage.getItem(EXPIRES_KEY);
    if (!token || !expiresAt) {
      return false;
    }
    return new Date(expiresAt).getTime() > Date.now();
  }
}

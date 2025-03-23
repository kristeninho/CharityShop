import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenSubject = new BehaviorSubject<string | null>(null);
  public token$ = this.tokenSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>('/api/auth/login', { username, password })
      .pipe(
        tap((response) => {
          this.tokenSubject.next(response.token); // Store the token
          localStorage.setItem('token', response.token); // Save token to localStorage
        })
      );
  }

  logout() {
    this.tokenSubject.next(null); // Clear the token
    localStorage.removeItem('token'); // Remove token from localStorage
    this.router.navigate(['/login']); // Redirect to login page
  }

  getToken(): string | null {
    return this.tokenSubject.value || localStorage.getItem('token'); // Retrieve the token
  }
}
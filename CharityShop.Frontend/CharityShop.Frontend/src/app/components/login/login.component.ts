import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.errorMessage = ''; // Clear any previous error message

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        // Login successful, navigate to a protected route
        this.router.navigate(['/navigation']);
      },
      error: (err) => {
        // Handle login error
        this.errorMessage = 'Login failed. Please check your credentials.';
        console.error('Login error:', err);
      },
    });
  }
}

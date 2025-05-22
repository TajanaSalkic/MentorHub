import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service'; 
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  serverError: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    this.serverError = null; 
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log("Login successful", response);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          if (error.status === 500 && typeof error.error === 'string') {
            
            this.serverError = 'Your account has not been approved yet. Please wait for an administrator to approve your account.';
          } else {
            this.serverError = 'Login failed. Please check your credentials.';
          }
        }
      });
    }
  }

  register(){
    this.router.navigate(['/register']);
  }
  
}

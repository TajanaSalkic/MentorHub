import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service'; // Adjust the path as necessary
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/[A-Z]/), Validators.pattern(/[0-9]/) ]],
      //treba dodati da se bira uloga
      roleId: [2, [Validators.required]] 
    });
  }

  getFieldError(fieldName: string, errorName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return !!(field && field.touched && field.errors?.[errorName]);
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (response) => {
          this.router.navigate(['/']); 
        },
        error: (error) => {
          console.error('Registration failed', error);
        }
      });
    }
  }

  login(){
    this.router.navigate(['/']);
  }
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { QuillModule } from 'ngx-quill';

interface Student {
  id: number;
  name: string;
  surname: string;
  email: string;
}

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NavbarComponent, QuillModule],
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.css'
})
export class CreateProjectComponent implements OnInit {
  projectForm: FormGroup;
  students: Student[] = [];
  showStudentDropdown = false;
  selectedStudent: Student | null = null;
  
  

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.projectForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      startDate: [this.formatDate(new Date()), Validators.required],
      endDate: [this.formatDate(new Date()), Validators.required],
      points: [0, [Validators.required, Validators.min(0)]],
      url: ['', Validators.required],
      studentID: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.fetchStudents();
  }

  fetchStudents() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<{users: Student[]}>('https://localhost:7035/api/students', { headers }).subscribe({
      next: (response) => {
        this.students = response.users;
      },
      error: (error) => {
        console.error('Failed to fetch students', error);
      }
    });
  }

  toggleStudentDropdown() {
    this.showStudentDropdown = !this.showStudentDropdown;
  }

  selectStudent(student: Student) {
    this.selectedStudent = student;
    this.projectForm.patchValue({
      studentID: student.id
    });
    this.showStudentDropdown = false;
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSubmit() {
    if (this.projectForm.valid) {
      const token = localStorage.getItem('token');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      const formValue = {...this.projectForm.value};
      formValue.startDate = new Date(formValue.startDate).toISOString();
      formValue.endDate = new Date(formValue.endDate).toISOString();
      
      this.http.post('https://localhost:7035/api/projects', formValue, { headers }).subscribe({
        next: (response) => {
          console.log('Project created successfully', response);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.error('Failed to create project', error);
        }
      });
    } else {
      Object.keys(this.projectForm.controls).forEach(key => {
        this.projectForm.get(key)?.markAsTouched();
      });
    }
  }
}
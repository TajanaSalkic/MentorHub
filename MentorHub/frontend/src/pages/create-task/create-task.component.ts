import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { jwtDecode } from 'jwt-decode';


interface Student {
  id: number;
  name: string;
  surname: string;
  email: string;
}

interface DecodedToken {
  userId: string;
  role: string;
  [key: string]: any;
}

@Component({
  selector: 'app-create-task',
  imports: [FormsModule, CommonModule, ReactiveFormsModule, NavbarComponent, SidebarComponent],
  templateUrl: './create-task.component.html',
  styleUrl: './create-task.component.css'
})
export class CreateTaskComponent {

  projectForm!: FormGroup;
  students: Student[] = [];
  showStudentDropdown = false;
  selectedStudent: Student | null = null;
  projectId: number = 0;
  role: string = '';
  studentId:string = '';

  
  

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) {
    
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
  
      this.projectForm = this.fb.group({
        title: ['', Validators.required],
        description: ['', Validators.required],
        startDate: [this.formatDate(new Date()), Validators.required],
        endDate: [this.formatDate(new Date()), Validators.required],
        points: [0, [Validators.required, Validators.min(0)]],
        projectId: this.projectId,
        studentID: [null, Validators.required]
      });
    });

    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded = jwtDecode<DecodedToken>(token);
        this.role = decoded.role;
        this.studentId = decoded.userId;
        console.log(this.studentId);
      } catch (e) {
        console.error('Invalid token', e);
      }
    }

    if (this.role === 'Student') {
      this.projectForm.get('studentID')!.setValue(this.studentId);
    } else {
      this.fetchStudents();          
    }

  }

  fetchStudents() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<{users: Student[]}>(`https://localhost:7035/api/students/${this.projectId}`, { headers }).subscribe({
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

      console.log(formValue);

      
        this.http.post('https://localhost:7035/api/tasks', formValue, { headers }).subscribe({
          next: (response) => {
            console.log('Task created successfully', response);
            this.router.navigate([`/project-board/${this.projectId}`]);

          },
          error: (error) => {
            console.error('Failed to create task', error);
          }
        });
      
      
    } else {
      Object.keys(this.projectForm.controls).forEach(key => {
        this.projectForm.get(key)?.markAsTouched();
      });
    }
  }
 
}

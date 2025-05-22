import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';


interface DecodedToken {
  userId: string;
  role: string;
  [key: string]: any;
}

@Component({
  selector: 'app-edit-task',
  imports: [CommonModule, FormsModule, SidebarComponent, NavbarComponent],
  templateUrl: './edit-task.component.html',
  styleUrl: './edit-task.component.css'
})
export class EditTaskComponent {
  task: any = {
    name: '',
    description: '',
    status: 0,
    link: '',
    startDate: '',
    endDate: '',
    assignedStudentId: ''
  };
  students: any[] = [];
  userRole: string = '';
  showStudentDropdown = false;
  studentId:string = '';
  projectId: string='';

  get selectedStudentName(): string {
    const student = this.students.find(s => s.id === this.task.assignedStudentId);
    return student ? `${student.name} ${student.surname}` : 'Name Surname';
  }

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    this.projectId = this.route.snapshot.params['id'];
    this.loadUserRoleFromTokenAndStudents();
    this.loadProject(this.projectId);
  }

  loadProject(id: string) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.get(`https://localhost:7035/api/tasks/${id}`, { headers })
      .subscribe({
        next: (response: any) => {
          this.task = response.task;
          this.task.startDate = this.task.startDate.substring(0, 10);
          this.task.endDate = this.task.endDate.substring(0, 10);
          console.log(this.task);
        },
        error: (error) => console.error('Error loading project:', error)
      });
  }

  loadUserRoleFromTokenAndStudents() {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        this.userRole = decoded.role || '';
        this.studentId = decoded.userId;
        console.log("Role", this.userRole);
        if (this.userRole === 'Mentor') {
          this.loadStudents();
        }else if (this.userRole === 'Student') {
          this.task.assignedStudentId = this.studentId; 
        } 
      } catch (e) {
        this.userRole = '';
      }
    }
  }

  loadStudents() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.get<{users: any[]}>('https://localhost:7035/api/students', { headers })
      .subscribe({
        next: (response) => {
          this.students = response.users;
        },
        error: (error) => console.error('Error loading students:', error)
      });
  }

  onSubmit() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.put(`https://localhost:7035/api/tasks/${this.task.id}`, this.task, { headers })
      .subscribe({
        next: () => {
          this.router.navigate([`/task/${this.projectId}/${this.task.id}`]);
        },
        error: (error) => console.error('Error updating project:', error)
      });
  }

  toHome(){
    this.router.navigate(['/home']);
  }

  selectStudent(student: any) {
    this.task.assignedStudentId = student.id;
    this.showStudentDropdown = false;
  }
}

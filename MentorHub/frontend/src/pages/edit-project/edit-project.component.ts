import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { jwtDecode } from 'jwt-decode';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-edit-project',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, NavbarComponent],
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css']
})
export class EditProjectComponent implements OnInit {
  project: any = {
    name: '',
    description: '',
    status: 0,
    url: '',
    startDate: '',
    endDate: '',
    studentID:'',
    userID: ''
  };
  students: any[] = [];
  userRole: string = '';
  showStudentDropdown = false;

  get selectedStudentName(): string {
    const student = this.students.find(s => s.id === this.project.userID);
    return student ? `${student.name} ${student.surname}` : 'Name Surname';
  }

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private snackbar: SnackbarService
  ) {}

  ngOnInit() {
    const projectId = this.route.snapshot.params['id'];
    this.loadProject(projectId);
    this.loadUserRoleFromTokenAndStudents();
  }

  loadProject(id: string) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.get(`https://localhost:7035/api/projects/${id}`, { headers })
      .subscribe({
        next: (response: any) => {
          const project = response.project;

          if (project.startDate) {
            project.startDate = project.startDate.substring(0, 10);
          }
          if (project.endDate) {
            project.endDate = project.endDate.substring(0, 10);
          }

          this.project = project;

          this.project.userID = project.userID;
          console.log(this.project);
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
        console.log("Role", this.userRole);
        if (this.userRole === 'Mentor') {
          this.loadStudents();
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
    this.project.studentID = this.project.userID;
    console.log(this.project);
    this.http.put(`https://localhost:7035/api/projects/${this.project.id}`, this.project, { headers })
      .subscribe({
        next: () => {
          this.snackbar.showSuccess('Project updated successfully!')
          this.router.navigate([`/project/${this.project.id}`]);
        },
        error: (error) => this.snackbar.showError('Failed to update project.')

      });
  }

  toHome(){
    this.router.navigate(['/home']);
  }

    selectStudent(student: any) {
      this.project.userID = student.id;
      this.showStudentDropdown = false;
    }

} 
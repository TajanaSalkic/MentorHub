import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { AssignProjectModalComponent } from '../../components/assign-project-modal/assign-project-modal.component';

@Component({
  selector: 'app-assigned-students',
  imports: [CommonModule, NavbarComponent, SidebarComponent, AssignProjectModalComponent],
  templateUrl: './assigned-students.component.html',
  styleUrl: './assigned-students.component.css'
})
export class AssignedStudentsComponent {
  projects: any[] = []; 
  students: any[] = [];
  searchTerm: string = '';
  isModalOpen: boolean = false;
  projectId:number = 0;


  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
    });
    this.fetchProjects(); 
    this.fetchStudents();
  }

  

  fetchProjects() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>('https://localhost:7035/api/projects', { headers }).subscribe({
      next: (response) => {
        this.projects = response.projects; 
        console.log(this.projects);
      },
      error: (error) => {
        console.error('Failed to fetch mentors:', error);
      }
    });
  }

  fetchStudents() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>('https://localhost:7035/api/students', { headers }).subscribe({
      next: (response) => {
        this.students = response.users; 
        console.log(this.students);
        return this.students;
      },
      error: (error) => {
        console.error('Failed to fetch mentors:', error);
      }
    });
  }


  openAssignModal() {
    this.isModalOpen = true;
  }

  assignProject(event: { projectId: number; studentId: number; callback: (message: string) => void }) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  console.log("Mentor",event.projectId);
  console.log(event.studentId);
    this.http.post<any>('https://localhost:7035/api/assignproject', {
      projectId: event.projectId,
      userID: event.studentId
    }, { headers }).subscribe({
      next: (response) => {
        
        if (response.message === "This project and student are already paired.") {
          event.callback(response.message);
        } else if(response.message === "This project is already assigned to another student."){
          event.callback(response.message);
        }else {
          event.callback("Mentor assigned successfully.");
          this.isModalOpen = false;
          this.fetchStudents();
        }
      },
      error: (error) => {
        const message = error?.error?.message || 'Failed to assign project.';
        event.callback(message);
        console.error('Mentor assignment error:', error);
      }
    });
  }
  

  downloadPDF(userID:number) {

    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.http.get<any>(`https://localhost:7035/api/user/${userID}/projects-report`, { headers }).subscribe({
      next: (response) => {
        const base64Pdf = response.pdfContent;
  
        const byteCharacters = atob(base64Pdf);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'application/pdf' });
  
        const blobUrl = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = blobUrl;
        a.download = `${response.fileName}.pdf`;
        a.click();
  
        window.URL.revokeObjectURL(blobUrl);
      },
      error: (error) => {
        console.error('Failed to fetch PDF:', error);
      }
    });
  }
}

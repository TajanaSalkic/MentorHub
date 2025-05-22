import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';

@Component({
  selector: 'app-task-changes-table',
  imports: [CommonModule, NavbarComponent, SidebarComponent],
  templateUrl: './task-changes-table.component.html',
  styleUrl: './task-changes-table.component.css'
})
export class TaskChangesTableComponent {
  
  taskChanges: any[] = [];
  searchTerm: string = '';
  projectId:number = 0;
  

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
    });
    this.fetchStudents();
  }


    generatePdf() {
      const token = localStorage.getItem('token');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
      this.http.get<any>(`https://localhost:7035/api/user/${this.projectId}/projects-taskchanges`, { headers }).subscribe({
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
          a.download = `${response.filename}`;
          a.click();
    
          window.URL.revokeObjectURL(blobUrl);
        },
        error: (error) => {
          console.error('Failed to fetch PDF:', error);
        }
      });
    }
    

  fetchStudents() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>(`https://localhost:7035/api/taskchanges/${this.projectId}`, { headers }).subscribe({
      next: (response) => {
        this.taskChanges = response.taskChanges; 
        console.log(this.taskChanges);
        
      },
      error: (error) => {
        console.error('Failed to fetch taskChanges:', error);
      }
    });
  }

 
}

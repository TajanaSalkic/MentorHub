import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';

export enum ProjectStatus {
  Planning = 0,
  Active = 1,
  OnHold = 2,
  Completed = 3
}

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [CommonModule, SidebarComponent, NavbarComponent],
  templateUrl: './project-dashboard.component.html',
  styleUrl: './project-dashboard.component.css'
})
export class ProjectDashboardComponent implements OnInit {
  project: any = {};
  projectId: number = 0;
  
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
      this.fetchProjectDetails(this.projectId);
    });
  }

  fetchProjectDetails(id: number) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get(`https://localhost:7035/api/projects/${id}`, { headers }).subscribe({
      next: (response: any) => {
        this.project = response.project;
      },
      error: (error) => {
        console.error('Failed to fetch project details', error);
      }
    });
  }

  getStatusString(status: number): string {
    return ProjectStatus[status];
  }
  
  getStatusClass(status: number): string {
    const statusString = ProjectStatus[status].toLowerCase();
    return `status-${statusString}`;
  }

  editProject(projectId: number) {
    this.router.navigate(['/edit-project', projectId]);
  }
}
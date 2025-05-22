import { Component, Input, OnInit, Pipe, PipeTransform, SimpleChanges } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';

export enum ProjectStatus {
  Planning = 0,
  Active = 1,
  OnHold = 2,
  Completed = 3
}


@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, NavbarComponent, FormsModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {
  projects: any[] = [];
  filteredProjects: any[] = [];
  selectedStatus: number | null = null;
  statusList = [
    { label: 'Planning', value: 0 },
    { label: 'Active', value: 1},
    { label: 'On Hold', value: 2 },
    { label: 'Completed', value: 3},
  ];

  userRole: string = '';


  
  @Input() searchTerm: string = '';  

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        this.userRole = decoded.role || '';
        console.log("Role", this.userRole);
      } catch (e) {
        this.userRole = '';
      }
    }
    this.fetchProjects();
  }

  fetchProjects() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get('https://localhost:7035/api/projects', { headers }).subscribe({
      next: (response: any) => {
        this.projects = response.projects;
        this.filteredProjects = response.projects; 
        console.log(this.projects);
      },
      error: (error) => {
        console.error('Failed to fetch projects', error);
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['searchTerm']) {
      this.filterProjects();
    }
  }

  filterProjects() {
    const searchLower = this.searchTerm.toLowerCase();

    this.filteredProjects = this.projects.filter(project =>
      Object.values(project).some(value =>
        value && value.toString().toLowerCase().includes(searchLower)
      )
    );
    console.log(this.filteredProjects);

  }

  

  getStatusString(status: number): string {
    return ProjectStatus[status];
  }

  getStatusClass(status: number): string {
    return `status-${ProjectStatus[status].toLowerCase()}`;
  }

  navigateToProject(projectId: number) {
    this.router.navigate(['/project', projectId]);
  }

  createProject(){
    this.router.navigate(['/create-project'])
  }

  editProject(projectId: number) {
    this.router.navigate(['/edit-project', projectId]);
  }
}

import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { AuthService } from '../../services/auth.service';
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

@Pipe({
  name: 'filterByStatus',
  standalone: true
})
export class FilterByStatusPipe implements PipeTransform {
  transform(projects: any[], status: number): any[] {
    return projects.filter(project => project.status === status);
  }
}

@Pipe({
  name: 'count',
  standalone: true
})
export class CountPipe implements PipeTransform {
  transform(projects: any[]): number {
    return projects.length;
  }
}

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, SidebarComponent, NavbarComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {
  projects: any[] = [];
  userName: string = 'Tajana Salkić';
  
  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    this.fetchProjects();
  }

  fetchProjects() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get('https://localhost:7035/api/projects', { headers }).subscribe({
      next: (response: any) => {
        this.projects = response.projects;
      },
      error: (error) => {
        console.error('Failed to fetch projects', error);
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
}

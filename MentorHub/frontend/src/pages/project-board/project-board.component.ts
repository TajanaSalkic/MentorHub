import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { CdkDragDrop, DragDropModule } from '@angular/cdk/drag-drop';

export enum ProjectStatus {
  Planning = 'Planning',
  Active = 'Active',
  OnHold = 'OnHold',
  Completed = 'Completed'
}

@Component({
  selector: 'app-project-board',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SidebarComponent,
    NavbarComponent,
    DragDropModule
  ],
  templateUrl: './project-board.component.html',
  styleUrl: './project-board.component.css'
})
export class ProjectBoardComponent implements OnInit {
  projectId: number = 0;
  searchTerm: string = '';
  columns: { label: string; status: number; tasks: any[] }[] = [
    { label: 'Planning', status: 0, tasks: [] },
    { label: 'Active', status: 1, tasks: [] },
    { label: 'Done', status: 2, tasks: [] },
    { label: 'On hold', status: 3, tasks: [] }
  ];
  isLoading: boolean = true;
  error: string | null = null;
  statuses = [
    { value: 0, label: 'Planning' },
    { value: 1, label: 'Active' },
    { value: 2, label: 'Done' },
    { value: 3, label: 'On hold' }
  ];

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) {}

  getConnectedLists(): string[] {
    return this.columns.map((_, i) => `column-${i}`);
  }
  viewTaskDetails(taskId: number): void {
    this.router.navigate(['/task', this.projectId, taskId]);
  }
  onDrop(event: CdkDragDrop<any[]>) {
    if (event.previousContainer === event.container) {
      return;
    }
    const task = event.item.data;
    const newColumnIndex = parseInt(event.container.id.split('-')[1]);
    const newStatus = this.columns[newColumnIndex].status;
    this.updateTaskStatus(task, newStatus, newColumnIndex);
  }
  updateTaskStatus(task: any, newStatus: number, newColumnIndex: number) {
    const token = localStorage.getItem('token');
    if (!token) {
      this.error = 'Authentication token not found';
      return;
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.isLoading = true;
    this.http.put('https://localhost:7035/api/tasks/changestatus', {
      id: task.id,
      projectStatus: newStatus
    }, { headers }).subscribe({
      next: () => {
        for (const col of this.columns) {
          col.tasks = col.tasks.filter((t: any) => t.id !== task.id);
        }
        const updatedTask = { ...task, status: newStatus };
        this.columns[newColumnIndex].tasks.push(updatedTask);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to update task status:', error);
        this.error = 'Failed to update task status';
        this.isLoading = false;
        this.fetchTasks();
      }
    });
  }

  ngOnInit() {
    try {
      this.projectId = +this.route.snapshot.params['id'];
      if (isNaN(this.projectId)) {
        throw new Error('Invalid project ID');
      }
      this.fetchTasks();
    } catch (error) {
      this.error = 'Failed to initialize project board';
      this.isLoading = false;
    }
  }
  fetchTasks() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.error = 'Authentication token not found';
      this.isLoading = false;
      return;
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    console.log('Fetching tasks for project:', this.projectId);
    this.http.get<any>(`https://localhost:7035/api/project/${this.projectId}/tasks`, { headers })
      .subscribe({
        next: (response) => {
          if (response && Array.isArray(response)) {
            for (const col of this.columns) {
              col.tasks = response.filter((t: any) => t.status === col.status);
            }
          } else if (response && response.tasks && Array.isArray(response.tasks)) {
            for (const col of this.columns) {
              col.tasks = response.tasks.filter((t: any) => t.status === col.status);
            }
          } else {
            console.error('Unexpected response format:', response);
            this.error = 'Invalid response format from server';
          }
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error fetching tasks:', error);
          this.error = 'Failed to fetch tasks';
          this.isLoading = false;
        }
      });
  }
  createTask() {
    this.router.navigate([`create-task/project/${this.projectId}`]);
  }
  changeTaskStatus(task: any, colIdx: number) {
    const newStatus = task.status; 
    const newColumnIndex = this.columns.findIndex(col => col.status === newStatus);
    this.updateTaskStatus(task, newStatus, newColumnIndex);
  }
  getStatusClass(status: number): string {
    switch (status) {
      case 0: return 'status-planning';
      case 1: return 'status-active';
      case 2: return 'status-done';
      case 3: return 'status-onhold';
      default: return '';
    }
  }
}




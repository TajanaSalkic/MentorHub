import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { QuillModule } from 'ngx-quill';

export enum TaskStatus {
  Planning = 0,
  Active = 1,
  Done = 2,
  OnHold = 3
}

interface Comment {
  id: number;
  content: string;
  createdDate: string | Date;
  userId: number;
  name: string;
  surname: string;
}

interface CommitLink {
  id: number;
  url: string;
}

interface Task {
  id: number;
  title: string;
  description: string;
  startDate: string | Date;
  endDate: string | Date;
  status: TaskStatus;
  points: number;
}

@Component({
  selector: 'app-task-dashboard',
  standalone: true,
  imports: [CommonModule, SidebarComponent, NavbarComponent, FormsModule, QuillModule],
  templateUrl: './task-dashboard.component.html',
  styleUrl: './task-dashboard.component.css'
})
export class TaskDashboardComponent implements OnInit {
  taskId: number = 0;
  task: Task = {} as Task;
  comments: Comment[] = [];
  commitLinks: CommitLink[] = [];
  isLoading: boolean = true;
  error: string | null = null;
  newComment: string = '';
  showCommentEditor: boolean = false;
  editingCommentId: number | null = null;
  editingCommentContent: string = '';
  currentUserId: number = 0;
  newCommitLink: string = '';
  showCommitLinkInput: boolean = false;
  projectId:number=0;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.taskId = +params['taskId'];
      this.projectId = +params['id'];
      this.fetchTaskDetails(this.taskId);
      this.fetchComments(this.taskId);
      this.fetchCommitLinks(this.taskId);
      this.getCurrentUserId();
    });
  }

  getCurrentUserId(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        this.currentUserId = decodedToken.userId || 0;
      } catch (error) {
        console.error('Invalid token:', error);
      }
    }
  }

  fetchTaskDetails(id: number): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.isLoading = true;
    this.http.get(`https://localhost:7035/api/tasks/${id}`, { headers }).subscribe({
      next: (response: any) => {
        this.task = response.task;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to fetch task details', error);
        this.error = 'Failed to load task details. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  fetchComments(taskId: number): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<{comments: Comment[]}>(`https://localhost:7035/api/tasks/${taskId}/comments`, { headers }).subscribe({
      next: (response) => {
        this.comments = response.comments;
      },
      error: (error) => {
        console.error('Failed to fetch comments', error);
      }
    });
  }

  fetchCommitLinks(taskId: number): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.get<{links: CommitLink[]}>(`https://localhost:7035/api/tasks/${taskId}/commits`, { headers }).subscribe({
      next: (response) => {
        this.commitLinks = response.links;
      },
      error: (error) => {
        console.error('Failed to fetch commit links', error);
      }
    });
  }

  getStatusString(status: number): string {
    return TaskStatus[status];
  }
  
  getStatusClass(status: number): string {
    const statusString = TaskStatus[status].toLowerCase();
    return `status-${statusString}`;
  }

  formatDate(date: string | Date): string {
    return new Date(date).toLocaleString();
  }

  addComment(): void {
    if (!this.newComment.trim()) return;
    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    const comment = {
      content: this.newComment,
      taskId: this.taskId,
      createdDate: new Date().toISOString()
    };
    
    this.http.post('https://localhost:7035/api/comments/add', comment, { headers }).subscribe({
      next: (response: any) => {
        this.comments.push(response.comment);
        this.newComment = '';
        this.showCommentEditor = false;
      },
      error: (error) => {
        console.error('Failed to add comment', error);
      }
    });
  }

  startEditComment(comment: Comment): void {
    this.editingCommentId = comment.id;
    this.editingCommentContent = comment.content;
  }

  updateComment(): void {
    if (!this.editingCommentId || !this.editingCommentContent.trim()) return;
    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    const comment = {
      id: this.editingCommentId,
      content: this.editingCommentContent
    };
    
    this.http.put(`https://localhost:7035/api/comments/${this.editingCommentId}`, comment, { headers }).subscribe({
      next: (response: any) => {
        const index = this.comments.findIndex(c => c.id === this.editingCommentId);
        if (index !== -1) {
          this.comments[index] = {
            ...this.comments[index],
            content: this.editingCommentContent
          };
        }
        this.editingCommentId = null;
        this.editingCommentContent = '';
      },
      error: (error) => {
        console.error('Failed to update comment', error);
      }
    });
  }

  deleteComment(commentId: number): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    this.http.delete(`https://localhost:7035/api/comments/${commentId}`, { 
      headers
    }).subscribe({
      next: () => {
        this.comments = this.comments.filter(c => c.id !== commentId);
      },
      error: (error) => {
        console.error('Failed to delete comment', error);
      }
    });
  }

  addCommitLink(): void {
    if (!this.newCommitLink.trim()) return;
    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    const commitLink = {
      url: this.newCommitLink,
      taskId: this.taskId
    };
    
    this.http.post(`https://localhost:7035/api/tasks/${this.taskId}/commits`, commitLink, { headers }).subscribe({
      next: (response: any) => {
        this.commitLinks.push({
          id: response.id,
          url: this.newCommitLink
        });
        this.newCommitLink = '';
        this.showCommitLinkInput = false;
      },
      error: (error) => {
        console.error('Failed to add commit link', error);
      }
    });
  }

  cancelEditComment(): void {
    this.editingCommentId = null;
    this.editingCommentContent = '';
  }

  isCommentOwner(userId: number): boolean {
    console.log("Comment user Id:", userId, typeof userId);
console.log("Current user:", this.currentUserId, typeof this.currentUserId);

    console.log(userId === this.currentUserId);
    return userId === Number(this.currentUserId);
  }

  editTask(taskId: number) {
    this.router.navigate(['/edit-task', this.projectId, taskId]);
  }

  deleteTask(taskId: number) {
    const confirmed = window.confirm('Are you sure you want to delete this task?');

    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    if (confirmed) {
      this.http.delete(`https://localhost:7035/api/tasks/${taskId}`, { headers }).subscribe({
        next: () => {
          alert('Task successfully deleted.');
          this.router.navigate(['/project-board', this.projectId]);
        },
        error: (err) => {
          console.error('Error deleting task:', err);
          this.error = 'Failed to delete task. Please try again later.';
        }
      });
    }
  }
  
}

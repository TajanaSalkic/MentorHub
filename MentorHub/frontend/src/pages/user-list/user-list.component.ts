import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';
import { AssignMentorModalComponent } from '../../components/assign-mentor-modal/assign-mentor-modal.component';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule, NavbarComponent, SidebarComponent, AssignMentorModalComponent],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: any[] = [];
  mentors: any[] = []; 
  students: any[] = [];
  searchTerm: string = '';
  isModalOpen: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.fetchUsers();
    this.fetchMentors(); 
    this.fetchStudents();
  }

  fetchUsers() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>('https://localhost:7035/api/users', { headers }).subscribe({
      next: (response) => {
        this.users = response.users; 
        console.log(this.users);
      },
      error: (error) => {
        console.error('Failed to fetch users:', error);
      }
    });
  }

  fetchMentors() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>('https://localhost:7035/api/mentors', { headers }).subscribe({
      next: (response) => {
        this.mentors = response.users; 
        console.log(this.mentors);
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

  assignMentor(event: { mentorId: number; studentId: number; callback: (message: string) => void }) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  console.log("Mentor",event.mentorId);
  console.log(event.studentId);
    this.http.post<any>('https://localhost:7035/api/mentorship', {
      mentor_ID: event.mentorId,
      student_ID: event.studentId
    }, { headers }).subscribe({
      next: (response) => {
        
        if (response.message === "This mentor and student are already paired.") {
          event.callback(response.message);
        } else {
          event.callback("Mentor assigned successfully.");
          this.isModalOpen = false;
          this.fetchUsers();
        }
      },
      error: (error) => {
        const message = error?.error?.message || 'Failed to assign mentor.';
        event.callback(message);
        console.error('Mentor assignment error:', error);
      }
    });
  }
 

  toggleApproval(user: any) {
    const token = localStorage.getItem('token');
    console.log("Token",token);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    const updatedApprovalStatus = !user.approved;
  
    this.http.put<any>(`https://localhost:7035/api/users/${user.id}/approve/${updatedApprovalStatus}`,{}, { headers }).subscribe({
      next: () => {
        user.approved = updatedApprovalStatus; 
      },
      error: (error) => {
        console.error('Failed to update approval status:', error);
      }
    });
  }
  
}
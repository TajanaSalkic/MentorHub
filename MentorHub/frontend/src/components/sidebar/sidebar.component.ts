import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';


interface DecodedToken {
  role: string;
  [key: string]: any;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  projectId: number = 0;
  role: string = '';

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
    });

    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded = jwtDecode<DecodedToken>(token);
        this.role = decoded.role;
      } catch (e) {
        console.error('Invalid token', e);
      }
    }
  }
  logout() {
    localStorage.removeItem('token'); 
    this.router.navigate(['']); 
  }

  board(){
    console.log("this is board: " + this.projectId);
    this.router.navigate([`/project-board/${this.projectId}`]); 
  }

  taskChanges(){
    this.router.navigate([`/task-changes/project/${this.projectId}`]); 
  }

  assignedStudents(){
    this.router.navigate([`assigned-students/${this.projectId}`]); 
  }

  users(){
    this.router.navigate([`/users/${this.projectId}`]);
  }

  home(){
    this.router.navigate([`/project/${this.projectId}`]);
  }
}

import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  userName: string = 'Guest';
  dropdownVisible: boolean = false; 
  userRole:string='';

  constructor(private router: Router) {}

  ngOnInit() {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = localStorage.getItem('token'); 
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token); 
        this.userRole = decodedToken.role;
        this.userName = `${decodedToken.name} ${decodedToken.surname}`; 
      } catch (error) {
        console.error('Invalid token:', error);
        this.userName = 'Guest';
      }
    }
  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible;
  }

  goToProjects() {
    this.router.navigate(['/home']); 
    this.toggleDropdown(); 
  }

  logout() {
    localStorage.removeItem('token'); 
    this.router.navigate(['']); 
  }

  goToUsers() {
    this.router.navigate(['/users/0']); 
    this.toggleDropdown(); 
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const clickedInside = (event.target as HTMLElement).closest('.user-info');
    if (!clickedInside) {
      this.dropdownVisible = false; 
    }
  }
}

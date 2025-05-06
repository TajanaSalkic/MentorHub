import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  userName: string = 'Guest';
  dropdownVisible: boolean = false; 

  constructor(private router: Router) {}

  ngOnInit() {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = localStorage.getItem('token'); 
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token); 

        console.log("Decodec",decodedToken);
        console.log("Name", decodedToken.name);
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

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const clickedInside = (event.target as HTMLElement).closest('.user-info');
    if (!clickedInside) {
      this.dropdownVisible = false; 
    }
  }
}

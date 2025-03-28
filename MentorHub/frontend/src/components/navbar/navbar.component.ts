import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  userName: string = 'Guest';

  constructor(private router: Router) {}

  ngOnInit() {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = localStorage.getItem('token'); 
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token); 

        console.log(decodedToken);
        this.userName = `${decodedToken.name} ${decodedToken.surname}`; 
      } catch (error) {
        console.error('Invalid token:', error);
        this.userName = 'Guest';
      }
    }
  }
}

import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {

    const expectedRoles: string[] = route.data['roles'];
    const token = localStorage.getItem('token');

    if (!token) {
      this.router.navigate(['/']);
      return false;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      const userRole = decodedToken.role;

      if (expectedRoles && expectedRoles.includes(userRole)) {
        return true;
      } else {
        this.router.navigate(['/home']); 
        return false;
      }
    } catch (err) {
      console.error('Token decoding failed', err);
      this.router.navigate(['/']);
      return false;
    }
  }
}

import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
    constructor(private router: Router, private authService: AuthService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
            let url: string = state.url;

            // if not logged in, and tries to access route that is not the login, return falsee
            if (!this.authService.isLoggedIn() && url !== '/' &&  url !== '') {
                this.router.navigate(['/']);
                return false;
            }

            if (url === '/users' || (route.queryParams && route.queryParams.userId)) {
                // TODO: csak adminként lehessen elérni ezeket a route-okat
                // this.router.navigate(['/error']);
            } else if (this.authService.isLoggedIn() && url === '/' || url === '') {
                // if navigates to login, and logged in, redirect to list
                this.router.navigate(['/list']);
            }
            return true;
    }
}
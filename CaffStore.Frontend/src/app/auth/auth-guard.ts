import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { AuthService } from './auth.service';

// https://angular.io/guide/router#resolve-pre-fetching-component-data

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
    constructor(private router: Router, private authService: AuthService) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
            let url: string = state.url;

            // if not logged in, and tries to access route that is not the login, return falsee
            if (!this.authService.isLoggedIn() && url !== '/' && url !== '') {
                return false;
            }

            if (url === '/users' || (route.queryParams && route.queryParams.userId)) {
                // TODO: csak adminként lehessen elérni ezeket a route-okat
                // this.router.navigate(['/error']);
            } else if (url === '/') {
                // if navigates to login, and logged in, redirect to list
                this.router.navigate(['/list']);
            }
            return true;
    }
}

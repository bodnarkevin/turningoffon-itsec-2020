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
            if (url === '/users' || (route.queryParams && route.queryParams.userId)) {
                // TODO: csak adminként lehessen elérni ezeket a route-okat
                // this.router.navigate(['/error']);
            } else if (url === '/') {
                if (this.authService.isLoggedIn()) {
                    this.router.navigate(['/list']);
                } else {
                    return true;
                }
            }
            return true;
    }
}

import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authService: AuthService) { }

    async canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Promise<boolean> {
            const url: string = state.url;

            // if not logged in, and tries to access route that is not the startpage, redirect to startpage            
            if (!this.authService.isLoggedIn() && url !== '/' &&  url !== '') {
                this.router.navigate(['/']);
                return false;
            }
            // only admin can access these routes
            if (this.authService.isLoggedIn() && (url === '/users' || (route.queryParams && route.queryParams.userId))) {
                await this.authService.isAdmin()
                    .then((res) => {
                        if (!res) {
                            this.router.navigate(['/error']);
                        }
                    })
                    .catch(() => {
                        this.router.navigate(['/error']);
                    });
            } else if (this.authService.isLoggedIn() && url === '/' || url === '') {
                // if navigates to login, and logged in, redirect to list
                this.router.navigate(['/list']);
            }
            return true;
    }
}

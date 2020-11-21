import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

// https://angular.io/guide/router#resolve-pre-fetching-component-data

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
    constructor(private router: Router) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
            let url: string = state.url;
            if (url === '/users' || (route.queryParams && route.queryParams.userId)) {
                // TODO: csak adminként lehessen elérni ezeket a route-okat
                // this.router.navigate(['/error']);
            } else if (url === '/') {
                // TODO: ha úgy navigál a loginra, hogy már be van jelentkezve, akkor irányítsuk át a listára
                // this.router.navigate(['/list']);
            }
            return true;
    }
}

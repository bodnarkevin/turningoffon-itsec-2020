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
    return this.canAccess(url);
  }

  canAccess(url: string): boolean {
    if (url === '/users' || url.includes('userId')) {
        // TODO: csak adminként lehessen elérni ezeket a route-okat
        this.router.navigate(['/error']);
        return false;
    }
    return true;
  }
}

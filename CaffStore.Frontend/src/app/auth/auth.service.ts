import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(private oAuthService: OAuthService) { }

    public getAccessToken(): string {
        return this.oAuthService.getAccessToken();
    }

    public isLoggedIn(): boolean {
        return this.oAuthService.hasValidAccessToken();
    }

    async isAdmin(): Promise<boolean> {
        return this.oAuthService.loadUserProfile().then(
            (res) => {
                if (res && res.role && res.role === 'Admin') {
                    return true;
                } else {
                    return false;
                }
            });
    }
}

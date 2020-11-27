import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    constructor(private oAuthService: OAuthService, private route: ActivatedRoute, private router: Router) { }

    public getAccessToken(): string {
        return this.oAuthService.getAccessToken();
    }

    public isLoggedIn(): boolean {
        return this.oAuthService.hasValidAccessToken();
    }

    async isAdmin(): Promise<boolean> {
        return await this.oAuthService
            .loadUserProfile()
                .then((res) => {
                    if (res && res.role && res.role === 'Admin') {
                        return true;
                    } else {
                        return false;
                    }
                })
                .catch((err) => {
                    // TODO
                    this.router.navigate([this.router.url]);
                    return false;
                });
    }

    async getCurrentUserEmail(): Promise<string> {
        return await this.oAuthService.loadUserProfile().then((res) => {
            if (res && res.name) {
                const email = res.name;
                return email;
            } else {
                console.log('email not found');
            }
        });
    }
}

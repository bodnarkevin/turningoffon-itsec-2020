import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../auth/auth.service';

@Component({
    selector: 'app-frame',
    templateUrl: './frame.component.html',
    styleUrls: ['./frame.component.css']
})
export class FrameComponent implements OnInit, OnChanges {

    @Input() title = '';
    menuOpened = false;
    isAdmin = false;

    constructor(private router: Router, private authService: AuthService, private oAuthService: OAuthService) { }

    ngOnInit(): void {
        this.authService.isAdmin().then((res) => {
            if (res) {
                this.isAdmin = true;
            } else {
                this.isAdmin = false;
            }
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes && changes.title && changes.title.currentValue !== changes.title.previousValue) {
            this.menuOpened  = false;
        }
    }

    onLogout(): void {
        // TODO: maybe revokeToken needed? nullreference
        // this.oAuthService.revokeTokenAndLogout();
        this.oAuthService.logOut(true);
        this.router.navigate(['/']);
    }
}

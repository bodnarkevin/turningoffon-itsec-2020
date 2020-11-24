import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
    selector: 'app-frame',
    templateUrl: './frame.component.html',
    styleUrls: ['./frame.component.css']
})
export class FrameComponent implements OnInit, OnChanges {

    @Input() title: string = '';
    menuOpened: boolean = false;
    isAdmin: boolean = false;

    constructor(private router: Router, private oAuthService: OAuthService) { }

    ngOnInit() {
        this.oAuthService.loadUserProfile().then((res) => {
            if (res && res.role && res.role === 'Admin') {
                this.isAdmin = true;
            } else {
                this.isAdmin = false;
            }
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes && changes.title && changes.title.currentValue !== changes.title.previousValue) {
            this.menuOpened  = false;
        }
    }

    onLogout(): void {
        // TODO: maybe revokeToken needed? 
        // this.oAuthService.revokeTokenAndLogout();
        this.oAuthService.logOut();
        this.router.navigate(['/']);
    }
}

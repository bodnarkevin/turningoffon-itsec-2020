import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    message: string = '';

    loginForm = new FormGroup({
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required)
    });

    constructor(private oAuthService: OAuthService) { }

    ngOnInit() { }

    onLogin(): void {
        this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(
            this.loginForm.controls.email.value, this.loginForm.controls.password.value)
            .then(tokenInfo => {
                console.log(this.message);
                this.message = JSON.stringify(tokenInfo);
            })
            .catch(() => {
                this.message = 'Invalid username or password';
            });
    }

    /*login(username: string, password: string): void {
        this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(username, password)
        .then(tokenInfo => {
            this.message = JSON.stringify(tokenInfo);
        })
        .catch(() => {
            this.message = 'Invalid username or password';
        });
    }*/

}

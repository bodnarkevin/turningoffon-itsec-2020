import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    loginForm = new FormGroup({
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required)
    });

    constructor(private oAuthService: OAuthService, private router: Router) { }

    ngOnInit() { }

    onLogin(): void {
        this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(
            this.loginForm.controls.email.value, this.loginForm.controls.password.value)
            .then((tokenInfo) => {
                console.log(tokenInfo);
                // TODO: tokenInfo role --> admin
                this.router.navigate(['/list']);
            })
            .catch(() => {
                alert('Invalid username or password');
            });
    }
}

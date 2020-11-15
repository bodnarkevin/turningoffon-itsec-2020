import {Component, OnInit} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public message: string;

  constructor(private oAuthService: OAuthService) {
  }

  ngOnInit(): void {
  }

  login(username: string, password: string): void {
    this.oAuthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(username, password)
      .then(tokenInfo => {
        this.message = JSON.stringify(tokenInfo);
      })
      .catch(() => {
        this.message = 'Invalid username or password';
      });
  }

}

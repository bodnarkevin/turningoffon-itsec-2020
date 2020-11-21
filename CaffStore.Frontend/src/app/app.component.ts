import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  showFrame: boolean = false;
  title: string = '';

  constructor(private oAuthService: OAuthService, private router: Router) {

    // keret komponenst a keződoldalon ne jelenítsük meg
    this.router.events.pipe(filter((event: any) => event instanceof NavigationEnd)).subscribe(event => {
        if (event.url == '/') {
          this.showFrame = false;
        } else {
            this.showFrame = true;
            
            switch(event.url) {
                case '/profile':
                    this.title = 'User profile';
                    break;
                case '/users':
                    this.title = 'Users';
                    break;
                case '/list':
                    this.title = 'CAFFs';
                    break;
                case '/my-caffs':
                    this.title = 'My images';
                    break;
                case '/caff':
                    this.title = 'CAFF details';
                    break;
                default:
                    this.title = '';
                    break;
            }
        }
      });

    // Required for password flow
    this.oAuthService.oidc = false;

    // The SPA's id. Register SPA with this id at the auth-server
    this.oAuthService.clientId = environment.auth.clientId;

    // set the scope for the permissions the client should request
    // The auth-server used here only returns a refresh token (see below), when the scope offline_access is requested
    this.oAuthService.scope = environment.auth.scope;

    // Use setStorage to use sessionStorage or another implementation of the TS-type Storage
    // instead of localStorage
    // TODO find safer method
    this.oAuthService.setStorage(localStorage);

    // Set the issuer of the token
    this.oAuthService.issuer = environment.auth.issuer;

    // Set a dummy secret
    // Please note that the auth-server used here demand the client to transmit a client secret, although
    // the standard explicitly cites that the password flow can also be used without it. Using a client secret
    // does not make sense for a SPA that runs in the browser. That's why the property is called dummyClientSecret
    // Using such a dummy secret is as safe as using no secret.
    // this.oauthService.dummyClientSecret = "geheim";

    // TODO Refine
    // Load Discovery Document and then try to login the user
    this.oAuthService.loadDiscoveryDocument(environment.auth.discoveryDocument).then(() => {
      // Try to refresh access token
      this.oAuthService.refreshToken().finally();
    });
  }
}

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { OAuthModule, OAuthService } from 'angular-oauth2-oidc';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ApiModule, Configuration } from './api/generated';
import { environment } from '../environments/environment';
import { AuthService } from './auth/auth.service';
import { FrameComponent } from './frame/frame.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    FrameComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    ApiModule,
    BrowserAnimationsModule 
  ],
  providers: [
    {
      provide: Configuration,
      useFactory: (authService: AuthService) => new Configuration(
        {
          basePath: environment.apiBasePath,
          accessToken: authService.getAccessToken.bind(authService)
        }
      ),
      deps: [AuthService],
      multi: false
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}

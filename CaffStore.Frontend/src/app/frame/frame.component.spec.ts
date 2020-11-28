import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AuthService } from '../auth/auth.service';

import { FrameComponent } from './frame.component';

describe('FrameComponent', () => {
  let component: FrameComponent;
  let auth: AuthService;
  let fixture: ComponentFixture<FrameComponent>;
  let navbar: FrameComponent;
  const falsyResponse = null;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FrameComponent ],
      imports: [
        RouterTestingModule,
        OAuthModule.forRoot(),
        HttpClientModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatDialogModule
       ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FrameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  beforeEach(() => {
    auth = TestBed.get(AuthService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Unauthenticated user', () => {

    beforeEach(() => {
      spyOn(auth, 'isLoggedIn').and.returnValue(falsyResponse);
      navbar = fixture.debugElement.componentInstance;
    });

    it('should receive a falsy response from auth.isLoggedIn', async(() => {
      navbar.ngOnInit();
      expect(auth.isLoggedIn()).toBeFalsy();
    }));
  });
});

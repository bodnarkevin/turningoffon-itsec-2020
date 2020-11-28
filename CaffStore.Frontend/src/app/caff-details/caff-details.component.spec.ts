import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { CaffDetailsComponent } from './caff-details.component';

describe('CaffDetailsComponent', () => {
  let component: CaffDetailsComponent;
  let fixture: ComponentFixture<CaffDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CaffDetailsComponent ],
      imports: [
        RouterTestingModule,
        HttpClientModule,
        OAuthModule.forRoot(),
        MatSnackBarModule
        ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CaffDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

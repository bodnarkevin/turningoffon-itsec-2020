import { HttpClientModule } from '@angular/common/http';
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { CaffDetailsComponent } from './caff-details.component';

describe('CaffDetailsComponent', () => {
  let component: CaffDetailsComponent;
  let fixture: ComponentFixture<CaffDetailsComponent>;
  let downloadButton: DebugElement;
  let deleteButton: DebugElement;
  let editButton: DebugElement;

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
    downloadButton = fixture.debugElement.query(By.css('#downloadButton'));
    deleteButton = fixture.debugElement.query(By.css('#deleteButton'));
    editButton = fixture.debugElement.query(By.css('#editButton'));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have downloadButton', () => {
    expect(downloadButton).toBeTruthy();
  });

  it('should not have deleteButton', () => {
    expect(deleteButton).toBeFalsy();
  });

  it('should not have editButton', () => {
    expect(editButton).toBeFalsy();
  });
});

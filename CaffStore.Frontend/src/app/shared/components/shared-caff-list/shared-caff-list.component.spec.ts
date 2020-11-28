import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterModule } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { SharedCaffListComponent } from './shared-caff-list.component';

describe('SharedCaffListComponent', () => {
  let component: SharedCaffListComponent;
  let fixture: ComponentFixture<SharedCaffListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SharedCaffListComponent ],
      imports: [
        RouterModule.forRoot([]),
        RouterTestingModule,
        HttpClientModule,
        MatDialogModule,
        OAuthModule.forRoot(),
        MatSnackBarModule],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} }
    ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SharedCaffListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should be on the first page', () => {
    expect(component.page).toBe(1);
  });

});

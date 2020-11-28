import { HttpClientModule } from '@angular/common/http';
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { By } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { SharedCaffListComponent } from './shared-caff-list.component';

describe('SharedCaffListComponent', () => {
  let component: SharedCaffListComponent;
  let fixture: ComponentFixture<SharedCaffListComponent>;
  let FAB: DebugElement;
  let filterButton: DebugElement;

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
    FAB = fixture.debugElement.query(By.css('#addFAB'));
    filterButton = fixture.debugElement.query(By.css('#filter'));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should be on the first page', () => {
    expect(component.page).toBe(1);
  });

  it('should not have filters', () => {
    expect(component.filters).toEqual([]);
  });

  it('should not have FAB button', () => {
    expect(FAB).toBeFalsy();
  });

  it('should have filter button', () => {
    expect(filterButton).toBeTruthy();
  });

  it('should open dialog', () => {
    filterButton.triggerEventHandler('click', null);
    expect(component.filterDialog.open).toBeTruthy();
  });

});

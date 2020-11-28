import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { StartpageComponent } from './startpage.component';

describe('StartpageComponent', () => {
  let component: StartpageComponent;
  let fixture: ComponentFixture<StartpageComponent>;
  let loginSection: DebugElement;
  let regSection: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StartpageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StartpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    loginSection = fixture.debugElement.query(By.css('#loginSection'));
    regSection = fixture.debugElement.query(By.css('#regSection'));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not have login section', () => {
    expect(loginSection).toBeTruthy();
  });

  it('should not have register section', () => {
    expect(regSection).toBeTruthy();
  });
});

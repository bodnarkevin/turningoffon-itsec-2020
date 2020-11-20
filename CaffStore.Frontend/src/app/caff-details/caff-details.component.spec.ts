import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaffDetailsComponent } from './caff-details.component';

describe('CaffDetailsComponent', () => {
  let component: CaffDetailsComponent;
  let fixture: ComponentFixture<CaffDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CaffDetailsComponent ]
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

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyCaffsComponent } from './my-caffs.component';

describe('MyCaffsComponent', () => {
  let component: MyCaffsComponent;
  let fixture: ComponentFixture<MyCaffsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyCaffsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyCaffsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaffListComponent } from './caff-list.component';

describe('CaffListComponent', () => {
  let component: CaffListComponent;
  let fixture: ComponentFixture<CaffListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CaffListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CaffListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

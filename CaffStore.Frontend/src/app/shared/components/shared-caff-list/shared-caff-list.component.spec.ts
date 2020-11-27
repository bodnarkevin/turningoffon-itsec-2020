import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedCaffListComponent } from './shared-caff-list.component';

describe('SharedCaffListComponent', () => {
  let component: SharedCaffListComponent;
  let fixture: ComponentFixture<SharedCaffListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SharedCaffListComponent ]
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
});

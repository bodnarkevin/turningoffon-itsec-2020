import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FilterCaffsDialogComponent } from './filter-caffs-dialog.component';


describe('FilterCaffsDialogComponent', () => {
  let component: FilterCaffsDialogComponent;
  let fixture: ComponentFixture<FilterCaffsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterCaffsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterCaffsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NewCaffDialogComponent } from './new-caff-dialog.component';


describe('NewCaffDialogComponent', () => {
  let component: NewCaffDialogComponent;
  let fixture: ComponentFixture<NewCaffDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewCaffDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCaffDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

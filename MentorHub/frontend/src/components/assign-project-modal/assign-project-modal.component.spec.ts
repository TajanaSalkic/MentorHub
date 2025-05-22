import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignProjectModalComponent } from './assign-project-modal.component';

describe('AssignProjectModalComponent', () => {
  let component: AssignProjectModalComponent;
  let fixture: ComponentFixture<AssignProjectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignProjectModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignProjectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

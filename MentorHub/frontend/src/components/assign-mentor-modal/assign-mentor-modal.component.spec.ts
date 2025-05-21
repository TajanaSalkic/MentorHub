import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignMentorModalComponent } from './assign-mentor-modal.component';

describe('AssignMentorModalComponent', () => {
  let component: AssignMentorModalComponent;
  let fixture: ComponentFixture<AssignMentorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignMentorModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignMentorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

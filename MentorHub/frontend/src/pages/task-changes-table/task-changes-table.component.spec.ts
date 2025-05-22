import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskChangesTableComponent } from './task-changes-table.component';

describe('TaskChangesTableComponent', () => {
  let component: TaskChangesTableComponent;
  let fixture: ComponentFixture<TaskChangesTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskChangesTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskChangesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

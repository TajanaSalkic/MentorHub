import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';


interface Student {
  id: number;
  name: string;
  surname: string;
  email: string;
}


@Component({
  selector: 'app-assign-project-modal',
  imports: [FormsModule, CommonModule],
  standalone: true,
  templateUrl: './assign-project-modal.component.html',
  styleUrl: './assign-project-modal.component.css'
})
export class AssignProjectModalComponent {
  @Input() projects: any[] = [];
  @Input() students: Student[] = [];
  @Output() closeModal = new EventEmitter<void>();
  @Output() assign = new EventEmitter<{ projectId: number; studentId: number; callback: (message: string) => void }>();

  selectedProject: number | null = null;
  selectedStudent: number | null = null;
  responseMessage: string = '';

  close() {
    this.closeModal.emit();
  }

  assignMentor() {
    if (this.selectedProject && this.selectedStudent) {
      this.assign.emit({
        projectId: this.selectedProject,
        studentId: this.selectedStudent,
        callback: (message: string) => {
          this.responseMessage = message;
        }
      });
    }
  }
}

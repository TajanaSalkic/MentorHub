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
  selector: 'app-assign-mentor-modal',
  imports: [FormsModule, CommonModule],
  standalone: true,
  templateUrl: './assign-mentor-modal.component.html',
  styleUrl: './assign-mentor-modal.component.css'
})
export class AssignMentorModalComponent {
  @Input() mentors: Student[] = [];
  @Input() students: Student[] = [];
  @Output() closeModal = new EventEmitter<void>();
  @Output() assign = new EventEmitter<{ mentorId: number; studentId: number; callback: (message: string) => void }>();

  selectedMentor: number | null = null;
  selectedStudent: number | null = null;
  responseMessage: string = '';

  close() {
    this.closeModal.emit();
  }

  assignMentor() {
    if (this.selectedMentor && this.selectedStudent) {
      this.assign.emit({
        mentorId: this.selectedMentor,
        studentId: this.selectedStudent,
        callback: (message: string) => {
          this.responseMessage = message;
        }
      });
    }
  }
}


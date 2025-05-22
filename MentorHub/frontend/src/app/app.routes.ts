import { Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RouterModule} from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { RegisterComponent } from '../pages/register/register.component';
import { HomePageComponent } from '../pages/home-page/home-page.component'; 
import { ProjectDashboardComponent } from '../pages/project-dashboard/project-dashboard.component';
import { CreateProjectComponent } from '../pages/create-project/create-project.component';
import { EditProjectComponent } from '../pages/edit-project/edit-project.component';
import { ProjectBoardComponent } from '../pages/project-board/project-board.component';
import { CreateTaskComponent } from '../pages/create-task/create-task.component';
import { UserListComponent } from '../pages/user-list/user-list.component';
import { AssignedStudentsComponent } from '../pages/assigned-students/assigned-students.component';
import { TaskDashboardComponent } from '../pages/task-dashboard/task-dashboard.component';
import { TaskChangesTableComponent } from '../pages/task-changes-table/task-changes-table.component';
import { EditTaskComponent } from '../pages/edit-task/edit-task.component';
import { AuthGuard } from './guards/auth.guard';


export const routes: Routes = [

    { 
        path: '', 
        component: LoginComponent ,
    },
    { 
        path: 'register', 
        component: RegisterComponent,
    },
    { 
        path: 'home', 
        component: HomePageComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']} 
    }, 
    { 
        path: 'project/:id', 
        component: ProjectDashboardComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']} 
    },
    { 
        path: 'create-project', 
        component: CreateProjectComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor']} 
    },
    { 
        path: 'edit-project/:id', 
        component: EditProjectComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor']}
    },
    { 
        path: 'project-board/:id', 
        component: ProjectBoardComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']}
    },
    { 
        path: 'create-task/project/:id', 
        component: CreateTaskComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']}
    },
    { 
        path: 'users/:id', 
        component: UserListComponent ,
        canActivate:[AuthGuard],
        data: {roles:['Admin']}
    },
    { 
        path: 'assigned-students/:id', 
        component: AssignedStudentsComponent,
        canActivate:[AuthGuard],
        data: {roles:['Mentor',]} 
    },
    { 
        path: 'task/:id/:taskId', 
        component: TaskDashboardComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']}
    },
    { 
        path: 'task-changes/project/:id', 
        component: TaskChangesTableComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']}
    },
    { 
        path: 'edit-task/:id/:taskId', 
        component: EditTaskComponent,
        canActivate:[AuthGuard],
        data: {roles:['Admin', 'Mentor', 'Student']}
    },



];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule {}

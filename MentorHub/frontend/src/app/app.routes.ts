import { Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RouterModule} from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { RegisterComponent } from '../pages/register/register.component';
import { HomePageComponent } from '../pages/home-page/home-page.component'; 
import { ProjectDashboardComponent } from '../pages/project-dashboard/project-dashboard.component';
import { CreateProjectComponent } from '../pages/create-project/create-project.component';


export const routes: Routes = [

    { path: '', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: HomePageComponent }, 
    { path: 'project/:id', component: ProjectDashboardComponent },
    { path: 'create-project', component: CreateProjectComponent },

];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule {}

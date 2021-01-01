import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from '../core/guards/auth.guard';

const routes: Routes = [
  {path: 'login', component: LoginComponent, data: { breadcrumb: 'Login' }},
  {path: 'register', component: RegisterComponent,  data: { breadcrumb: 'Register' }},
  {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard],  data: { breadcrumb: 'Register' }}
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ],
})
export class AccountRoutingModule { }

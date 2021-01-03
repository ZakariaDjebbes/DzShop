import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { RequestEmailConfirmationComponent } from './request-email-confirmation/request-email-confirmation.component';
import { RequetSuccessComponent } from './requet-success/requet-success.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent, data: { breadcrumb: 'Login' }},
  {path: 'register', component: RegisterComponent,  data: { breadcrumb: 'Register' }},
  {path: 'confirmEmail', component: ConfirmEmailComponent,  data: { breadcrumb: 'Confirm Email' }},
  {path: 'requestEmailConfirmation', component: RequestEmailConfirmationComponent,  data: { breadcrumb: 'Request Email Confirmation' }},
  {path: 'requestSuccess', component: RequetSuccessComponent,  data: { breadcrumb: 'Your request is successful' }},
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

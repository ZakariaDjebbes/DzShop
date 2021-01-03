import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ProfileComponent } from './profile/profile.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { RequestEmailConfirmationComponent } from './request-email-confirmation/request-email-confirmation.component';
import { RequetSuccessComponent } from './requet-success/requet-success.component';



@NgModule({
  declarations: [LoginComponent, RegisterComponent, ProfileComponent, ConfirmEmailComponent, RequestEmailConfirmationComponent, RequetSuccessComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule
  ]
})
export class AccountModule { }

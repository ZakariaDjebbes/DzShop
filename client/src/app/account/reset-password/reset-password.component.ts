import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  loading = false;
  errors = [];

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private router: Router,
              private activatedRoute: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.createResetPasswordForm();
  }

  onSubmit(): void {
    this.loading = true;
    const values = this.resetPasswordForm.value;
    values.token = this.activatedRoute.snapshot.queryParams.token;
    values.email = this.activatedRoute.snapshot.queryParams.email;
    console.log(values);
    this.accountService.resetPassword(values).subscribe(
      (res) => {
        const navigationExtras: NavigationExtras = { state: values.email };
        this.router.navigate(['/account/login'], navigationExtras);
        this.toastr.success('Your password has been correctly updated!', 'Success');
      },
      (error) => {
        console.error(error);
        this.errors = error.errors;
        this.loading = false;
      }
    );
  }

  createResetPasswordForm(): void {
    this.resetPasswordForm = this.formBuilder.group(
      {
        newPassword: [null, Validators.required],
        confirmNewPassword: [null, Validators.required],
      }, {validators: this.passwordsMatchValidator}
    );
  }

  // tslint:disable-next-line: typedef
  private passwordsMatchValidator(form: FormGroup) {
    if (form.get('newPassword') && form.get('confirmNewPassword')) {
      if (form.get('newPassword').value === form.get('confirmNewPassword').value) {
        return null;
      }
      else {
        form.get('confirmNewPassword').setErrors({ mismatch: true });
        return { mismatch: true };
      }
    }
    return null;
  }
}

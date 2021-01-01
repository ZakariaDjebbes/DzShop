import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { IAddress } from 'src/app/shared/models/address';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  accountForm: FormGroup;
  user$: Observable<IUser>;
  errors: [];
  loading = false;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.user$ = this.accountService.currentUser$;
    this.createAccountForm();
    this.getAddressForValues();
  }

  onProfileSubmit(): void {
    this.loading = true;
    this.accountService.updateUser(this.accountForm.get('profileForm').value).subscribe(
      (user) => {
        this.toastr.success('Your profile has been updated');
        this.errors = [];
        this.loading = false;
      },
      (error) => {
        console.error(error);
        this.errors = error.errors;
        this.toastr.error('Error updating your profile');
        this.loading = false;
      }
    );
  }

  onPasswordSubmit(): void {
      this.loading = true;

      this.accountService.updateUserPassword(this.accountForm.get('passwordForm').value).subscribe(
        (res) => {
          this.toastr.success('Your password has been updated');
          this.errors = [];
          this.loading = false;
        },
        (error) => {
          console.error(error);
          this.errors = error.errors;
          this.toastr.error('Error updating your password');
          this.loading = false;
        }
      );
  }

  onAddressSubmit(): void {
    this.loading = true;
    this.accountService.updateUserAddress(this.accountForm.get('addressForm').value)
    .subscribe((address: IAddress) => {
      this.toastr.success('Address saved');
      this.accountForm.get('addressForm').reset(address);
      this.errors = [];
      this.loading = false;
    }, error => {
      this.toastr.error(error.message);
      this.errors = error.errors;
      console.log(error);
      this.loading = false;
    });
  }

  private createAccountForm(): void {
    this.accountForm = this.formBuilder.group(
      {
        addressForm: this.formBuilder.group({
          firstName: [null, Validators.required],
          lastName: [null, Validators.required],
          country: [null, Validators.required],
          city: [null, Validators.required],
          zipcode: [null, Validators.required],
          street: [null, Validators.required],
        }),
        profileForm: this.formBuilder.group({
          userName: [null, [Validators.required], [this.validateUserNotTaken()]],
          email: [null,
            [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
            [this.validateEmailNotTaken()]
          ],
          password: [null, [Validators.required]],
          confirmPassword: [null, [Validators.required]]
        }, {validators: this.passwordsMatchValidator}),
        passwordForm: this.formBuilder.group({
          oldPassword: [null, [Validators.required]],
          confirmOldPassword: [null, [Validators.required]],
          newPassword: [null, [Validators.required]],
          confirmNewPassword: [null, [Validators.required]]
        }),
      });

    this.accountForm.get('profileForm').patchValue(this.getUserFromObservable());
  }

  getAddressForValues(): void {
    this.accountService.getUserAddress().subscribe(address => {
      if (address)
      {
        this.accountForm.get('addressForm').patchValue(address);
      }
    }, (error) => {
      console.error(error);
    });
  }


  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value || control.value === this.getUserFromObservable().email) { return of(null); }
          return this.accountService.checkEmailExists(control.value).pipe(map(res => res ? { emailExists: true } : null));
        })
      );
    };
  }

  validateUserNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value || control.value === this.getUserFromObservable().userName) { return of(null); }
          return this.accountService.checkUserExists(control.value).pipe(map(res => res ? { userExists: true } : null));
        })
      );
    };
  }

  // tslint:disable-next-line: typedef
  private passwordsMatchValidator(form: FormGroup) {
    if (form.get('password') && form.get('confirmPassword')) {
      if (form.get('password').value === form.get('confirmPassword').value) {
        return null;
      }
      else {
        form.get('confirmPassword').setErrors({ mismatch: true });
        return { mismatch: true };
      }
    }
    return null;
  }

  private getUserFromObservable(): IUser {
    let user: IUser;
    this.user$.subscribe(u => {
      user = u;
    });

    return user;
  }
}

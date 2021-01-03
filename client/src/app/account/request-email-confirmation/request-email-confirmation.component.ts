import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-request-email-confirmation',
  templateUrl: './request-email-confirmation.component.html',
  styleUrls: ['./request-email-confirmation.component.scss']
})
export class RequestEmailConfirmationComponent implements OnInit {
  confirmationForm: FormGroup;
  isLoading = false;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createConfirmationForm();
  }

  createConfirmationForm(): void {
    this.confirmationForm = this.formBuilder.group(
      {
        email: [null,
          [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
          [this.validateEmailTaken()]]
      }
    );
  }

  onSubmit(): void {
    this.isLoading = true;
    this.accountService.requestConfirmationEmail(this.confirmationForm.get('email').value).subscribe(
      (res) => {
        const navigationExtras: NavigationExtras = { state: this.confirmationForm.get('email').value };
        this.router.navigate(['/account/requestSuccess'], navigationExtras);
      },
      (error) => {
        console.error(error);
        this.isLoading = false;
      }
    );
  }

  validateEmailTaken(): any {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) { return of(null); }
          return this.accountService.checkEmailExists(control.value).pipe(map(res => !res ? { emailNotExists: true } : null));
        })
      );
    };
  }
}

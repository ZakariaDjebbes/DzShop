import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {
  isConfirmed = null;

  constructor(private activatedRoute: ActivatedRoute, private accountService: AccountService,
              private toastr: ToastrService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(
      (res) => {
        if (res.email && res.token) {
          this.confirmEmail(res.email, res.token);
        }
        else
        {
          this.isConfirmed = false;
        }
      },
      error => {
        console.error(error);
        this.isConfirmed = false;
      }
    );
  }

  confirmEmail(email: string, token: string): void {
    this.accountService.confirmEmail(token, email).subscribe(
      (res) => {
        if (res) {
          this.isConfirmed = true;
          this.toastr.success('Your email has been correctly confirmed! You can now login to your account');
        }
      },
      (error) => {
        console.error(error);
        this.isConfirmed = false;
      }
    );
  }
}

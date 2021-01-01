import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  loading = false;

  constructor(private accountService: AccountService, private router: Router,
              private activatedRoot: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoot.snapshot.queryParams.returnUrl || '/shop';
    this.createLoginForm();
  }

  private createLoginForm(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit(): void {
    this.loading = true;
    this.accountService.login(this.loginForm.value).subscribe(
      (user) => {
        this.router.navigateByUrl(this.returnUrl);
        this.toastr.success('Welcome back ' + user.userName);
      },
      error => {
      console.log(error);
      this.loading = false;
      },
      () => {
        this.loading = false;
      }
    );
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressForValues();
  }

  createCheckoutForm(): void {
    this.checkoutForm = this.formBuilder.group(
      {
        addressForm: this.formBuilder.group({
          firstName: [null, Validators.required],
          lastName: [null, Validators.required],
          country: [null, Validators.required],
          city: [null, Validators.required],
          zipcode: [null, Validators.required],
          street: [null, Validators.required],
        }),
        deliveryForm: this.formBuilder.group({
          deliveryMethod: [null, Validators.required]
        }),
        paymentForm: this.formBuilder.group({
          nameOnCard: [null, Validators.required]
        })
      }
    );
  }

  // tslint:disable-next-line: typedef
  getAddressForValues() {
    this.accountService.getUserAddress().subscribe(address => {
      if (address)
      {
        console.log(address);
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, (error) => {
      console.error(error);
    });
  }
}

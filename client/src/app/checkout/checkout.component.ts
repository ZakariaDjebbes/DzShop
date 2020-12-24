import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private basketService: BasketService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressForValues();
    this.getDeliveryMethod();
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

  getAddressForValues(): void {
    this.accountService.getUserAddress().subscribe(address => {
      if (address)
      {
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, (error) => {
      console.error(error);
    });
  }

  getDeliveryMethod(): void {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket.deliveryMethodId)
    {
      this.checkoutForm.get('deliveryForm').get('deliveryMethod').patchValue(basket.deliveryMethodId.toString());
    }
  }
}

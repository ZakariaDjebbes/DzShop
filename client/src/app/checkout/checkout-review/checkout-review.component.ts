import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  @Input() cdkStepper: CdkStepper;
  loading = false;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  // tslint:disable-next-line: typedef
  createPaymentIntent()
  {
    this.loading = true;
    return this.basketService.createPaymentIntent().subscribe(
      (res) => {
        this.cdkStepper.next();
      },
      (err) => {
        console.error(err);
        this.loading = false;
      },
      () => {
        this.loading = false;
      }
    );
  }

  backToDeliery(): void
  {
    this.cdkStepper.previous();
  }
}

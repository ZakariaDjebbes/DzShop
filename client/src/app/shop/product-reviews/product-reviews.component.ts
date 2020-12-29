import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { IReview } from 'src/app/shared/models/review';
import { IReviewToCreate } from 'src/app/shared/models/reviewToCreate';
import { ReviewParams } from 'src/app/shared/models/shopParams';
import { IUser } from 'src/app/shared/models/user';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-reviews',
  templateUrl: './product-reviews.component.html',
  styleUrls: ['./product-reviews.component.scss']
})
export class ProductReviewsComponent implements OnInit {
  @Input() maxCommentLength = 500;
  @Input() productId: number = null;

  reviewForm: FormGroup;
  max = 5;
  rate = 0;
  remainingCommentLength = this.maxCommentLength;
  errors: [];
  reviews: IReview[];
  reviewParams = new ReviewParams();
  user$: Observable<IUser>;
  loading = false;
  totalCount: number;

  constructor(private shopService: ShopService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.createReviewForm();
    this.user$ = this.accountService.currentUser$;
    this.getReviews();
  }

  private createReviewForm(): void {
    this.reviewForm = new FormGroup({
      comment: new FormControl('', Validators.maxLength(this.maxCommentLength)),
      stars: new FormControl(0, [Validators.required, Validators.min(1), Validators.max(5)])
    });
  }

  getRemainingCommentLength(): void
  {
    const currentLength = this.reviewForm.get('comment').value.length;
    this.remainingCommentLength = this.maxCommentLength - currentLength;
  }

  onSubmit(): void {
    this.loading = true;
    const comment: string = this.reviewForm.get('comment').value;
    const stars: number = this.reviewForm.get('stars').value;
    const reviewToCreate: IReviewToCreate = {
      comment,
      stars,
      productId: this.productId
    };
    this.shopService.reviewProduct(reviewToCreate).subscribe(
      (res: IReview) => {
        if (res)
        {
          this.getReviews();
          this.reviewForm.reset();
        }
      },
      (error) => {
        console.error(error);
        this.errors = error.errors;
        this.loading = false;
      },
      () => {
        this.loading = false;
      }
    );
  }

  getReviews(): void {
    this.reviewParams.pageSize = 5;
    this.shopService.getReviewsOfProduct(this.productId, this.reviewParams).subscribe(
      (res) => {
        if (res)
        {
          this.reviews = res.data;
          this.reviewParams.pageNumber = res.pageIndex;
          this.reviewParams.pageSize = res.pageSize;
          this.totalCount = res.count;
        }
      },
      (error) => {
        console.error(error);
      }
    );
  }

  public OnPageChanged(event: any): void {
    if (this.reviewParams.pageNumber !== event)
    {
      this.reviewParams.pageNumber = event;
      this.getReviews();
    }
  }
}

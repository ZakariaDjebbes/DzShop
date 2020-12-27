import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-reviews',
  templateUrl: './product-reviews.component.html',
  styleUrls: ['./product-reviews.component.scss']
})
export class ProductReviewsComponent implements OnInit {
  @Input() maxCommentLength = 500;
  @Input() isStarsReadonly = false;
  reviewForm: FormGroup;
  max = 5;
  rate = 0;
  remainingCommentLength = this.maxCommentLength;

  constructor() { }

  ngOnInit(): void {
    this.createReviewForm();
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
    console.log(this.getRemainingCommentLength());
  }
}

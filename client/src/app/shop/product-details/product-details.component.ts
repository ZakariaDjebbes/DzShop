import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopService: ShopService, private activatedRoot: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPrudct();
  }

  loadPrudct(): void {
    this.shopService.getProduct(+this.activatedRoot.snapshot.paramMap.get('id')).subscribe(prodcut => {
      this.product = prodcut;
    }, error => {
      console.error(error);
    });
  }
}
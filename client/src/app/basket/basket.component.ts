import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  public removeItem(item: IBasketItem): void {
    this.basketService.removeItem(item);
  }

  public incrementItem(item: IBasketItem): void {
    this.basketService.incrementItemQuantity(item);
  }

  public decrementItem(item: IBasketItem): void {
    this.basketService.decrementItemQuantity(item);
  }
}

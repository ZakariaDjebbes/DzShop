import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'MedSupply';

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    const basketId = localStorage.getItem('basket-id');

    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('basket initialized');
      }, error => {
        console.error(error);
      });
    }
  }
}

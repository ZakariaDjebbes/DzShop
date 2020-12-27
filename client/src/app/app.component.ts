import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'DzShop';

  constructor(private basketService: BasketService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadBasket();
    this.loadUser();
  }

  private loadBasket(): void {
    const basketId = localStorage.getItem('basket-id');

    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('basket initialized');
      }, error => {
        console.error(error);
      });
    }
  }

  private loadUser(): void {
    const token = localStorage.getItem('token');

    this.accountService.loadCurrentUser(token).subscribe(() => {
        console.log('user loaded');
      }, error => {
        console.error(error);
      });
  }
}

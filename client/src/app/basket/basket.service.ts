import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  shipping = 0.0;
  private basketSource = new BehaviorSubject<IBasket>(null);
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);

  public basket$ = this.basketSource.asObservable();
  public basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  public createPaymentIntent()
  {
    return this.http.post(this.baseUrl + 'payment/' + this.getCurrentBasketValue().id, {}).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
      })
    );
  }

  public setShippingPrice(deliveryMethod: IDeliveryMethod): void {
    this.shipping = deliveryMethod.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = deliveryMethod.id;
    this.calculateTotals();
    this.setBasket(basket);
  }

  // tslint:disable-next-line: typedef
  public getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);

        if (basket.deliveryMethodId !== null)
        {
          this.getDeliveryMethod(basket.deliveryMethodId).subscribe(
            (res) => {
              this.shipping = res.price;
            },
            (err) => {
              console.error(err);
            },
            () => {
              this.calculateTotals();
            }
          );
        }
        else
        {
          this.calculateTotals();
        }
      })
    );
  }

  // tslint:disable-next-line: typedef
  public setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe(
      (response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      error => {
        console.log(error);
      }
    );
  }

  // tslint:disable-next-line: typedef
  public deleteBasket(basket: IBasket) {
    this.shipping = 0.0;
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket-id');
    }, error => {
      console.log(error);
    });
  }

  // // tslint:disable-next-line: typedef
  // public deleteLocalBasket() {
  //   this.basketSource.next(null);
  //   this.basketTotalSource.next(null);
  //   this.shipping = 0.0;
  //   localStorage.removeItem('basket-id');
  // }

  // tslint:disable-next-line: typedef
  public addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductToBasketITem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

    this.setBasket(basket);
  }

  public incrementItemQuantity(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    const foundItem = basket.items.findIndex((x) => x.id === item.id);
    basket.items[foundItem].quantity++;
    this.setBasket(basket);
  }

  public decrementItemQuantity(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    const foundItem = basket.items.findIndex((x) => x.id === item.id);
    if (basket.items[foundItem].quantity > 1) {
      basket.items[foundItem].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItem(item);
    }
  }

  public getCurrentBasketValue(): IBasket {
    return this.basketSource.getValue();
  }

  public removeItem(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id))
    {
      basket.items = basket.items.filter(i => i.id !== item.id);

      if (basket.items.length > 0) {
        this.setBasket(basket);
      }
      else {
        this.deleteBasket(basket);
      }
    }
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket-id', basket.id);
    return basket;
  }

  // tslint:disable-next-line: typedef
  private getDeliveryMethod(id: number) {
    return this.http.get<IDeliveryMethod>(this.baseUrl + 'order/deliveryMethod/' + id);
  }

  private mapProductToBasketITem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
      quantity
    };
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);

    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }

    return items;
  }

  private calculateTotals(): void {
    const basket = this.getCurrentBasketValue();
    const subtotal = basket.items.reduce((value, item) => (item.quantity * item.price) + value, 0);
    const shipping = this.shipping;

    const total = subtotal + shipping;
    this.basketTotalSource.next({
      shipping,
      total,
      subtotal,
    });
  }
}

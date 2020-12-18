import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  public getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'order/deliveryMethods').pipe(
      map((dms: IDeliveryMethod[]) => {
        return dms.sort((a, b) => b.price - a.price);
      }, (error) => {
        console.error(error);
      })
    );
  }

  // tslint:disable-next-line: typedef
  public createOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl + 'order', order);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  public getOrders() {
    return this.http.get<IOrder[]>(this.baseUrl + 'order');
  }

  // tslint:disable-next-line: typedef
  public getOrder(orderId: number) {
    return this.http.get<IOrder>(this.baseUrl + `order/${orderId}`);
  }
}

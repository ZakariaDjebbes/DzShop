import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  order: IOrder;

  constructor(private activatedRoot: ActivatedRoute, private orderService: OrdersService, private breadCrumbServis: BreadcrumbService) {
    this.breadCrumbServis.set('@orderDetails', '');
  }

  ngOnInit(): void {
    this.getOrder();
  }

  private getOrder(): void{
    this.orderService.getOrder(+this.activatedRoot.snapshot.paramMap.get('id')).subscribe(
      (res) => {
        this.order = res;
        this.breadCrumbServis.set('@orderDetails', `Order ${this.order.id} - ${this.order.status}`);
      },
      (error) => {
        console.error(error);
      }
    );
  }
}

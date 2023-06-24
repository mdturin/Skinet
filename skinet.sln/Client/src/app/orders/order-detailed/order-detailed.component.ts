import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss'],
})
export class OrderDetailedComponent implements OnInit {
  order: IOrder;

  constructor(
    private route: ActivatedRoute,
    private bcService: BreadcrumbService,
    private orderService: OrdersService
  ) {
    this.bcService.set('@orderDetailed', '');
  }

  ngOnInit(): void {
    this.getOrderDetailed();
  }

  getOrderDetailed() {
    this.orderService
      .getOrderDetailed(+this.route.snapshot.paramMap.get('id'))
      .subscribe({
        next: (order: IOrder) => {
          this.order = order;
          this.bcService.set(
            '@orderDetailed',
            `Order# ${order.id} - ${order.status}`
          );
        },
        error: (error) => {
          console.log(error);
        },
      });
  }
}

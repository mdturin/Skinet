import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDeliveryMethod } from '../shared/models/delivery-method';
import { map } from 'rxjs';
import { IOrderToCreate } from '../shared/models/order';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  createOrder(order: IOrderToCreate) {
    const endpoint = this.baseUrl + 'orders';
    return this.http.post(endpoint, order);
  }

  getDeliveryMethods() {
    const endpoint = this.baseUrl + 'orders/deliveryMethods';
    return this.http.get(endpoint).pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }
}

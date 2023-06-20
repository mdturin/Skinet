import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDeliveryMethod } from '../shared/models/delivery-method';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getDeliveryMethods() {
    const endpoint = this.baseUrl + 'orders/deliveryMethods';
    return this.http.get(endpoint).pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }
}

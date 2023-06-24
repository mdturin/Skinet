import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';
import { Observable } from 'rxjs';
import { IBasketTotals } from '../shared/models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent {
  basketTotals$: Observable<IBasketTotals>;
  checkoutForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private basketService: BasketService
  ) {}

  ngOnInit() {
    this.createCheckoutForm();
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createCheckoutForm() {
    this.checkoutForm = this.formBuilder.group({
      addressForm: this.formBuilder.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipcode: [null, Validators.required],
      }),
      deliveryForm: this.formBuilder.group({
        deliveryMethod: [null, Validators.required],
      }),
      paymentForm: this.formBuilder.group({
        nameOnCard: [null, Validators.required],
      }),
    });
  }

  getAddressFormValues() {
    return this.accountService.getUserAddress().subscribe({
      next: (address) => {
        if (address) {
          this.checkoutForm.get('addressForm').patchValue(address);
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket.deliveryMethodId !== null) {
      return this.checkoutForm
        .get('deliveryForm')
        .get('deliveryMethod')
        .patchValue(basket.deliveryMethodId.toString());
    }
  }
}

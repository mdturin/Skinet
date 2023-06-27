import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';

declare var Stripe;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss'],
})
export class CheckoutPaymentComponent implements AfterViewInit, OnDestroy {
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', { static: true }) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', { static: true }) cardCvcElement: ElementRef;

  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  ngAfterViewInit(): void {
    this.stripe = Stripe(
      'pk_test_51NMPuMFJcAUl93qzME3wLYFwvt6e1ot5azvzqrqBeGHSW6rwj4DL4ez2r5wedniAap0Zi4i4Fs9rMBzWksPOmgig00lrHFpBPi'
    );
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler);
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: (order) => {
        this.toastr.success('Order created successfully');
        this.stripe
          .confirmCardPayment(basket.clientSecret, {
            payment_method: {
              card: this.cardNumber,
              billing_details: {
                name: this.checkoutForm.get('paymentForm').get('nameOnCard')
                  .value,
              },
            },
          })
          .then(({ error }) => {
            if (error) {
              this.toastr.error(error.message);
            } else {
              this.toastr.success('Order created successfully');
              this.basketService.deleteLocalBasket(basket.id);
              const navigationExtras: NavigationExtras = { state: order };
              this.router.navigate(['checkout/success'], navigationExtras);
            }
          });
      },
      error: (error) => {
        this.toastr.error(error.message);
        console.log(error);
      },
    });
  }

  onChange({ error }) {
    if (error) {
      this.cardErrors = error.message;
    } else {
      this.cardErrors = null;
    }
  }

  private getOrderToCreate(basket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm
        .get('deliveryForm')
        .get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value,
    };
  }
}

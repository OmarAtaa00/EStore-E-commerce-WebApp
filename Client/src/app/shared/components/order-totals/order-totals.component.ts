import { Component, OnInit } from '@angular/core';
import { CartService } from '../../../cart/cart.service';
import { Observable } from 'rxjs';
import { ICartTotal } from '../../models/cart';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss'],
})
export class OrderTotalsComponent implements OnInit {
  cartTotal$: Observable<ICartTotal>;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartTotal$ = this.cartService.cartTotal$;
  }
}

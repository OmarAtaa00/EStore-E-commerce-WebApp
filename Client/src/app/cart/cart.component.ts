import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ICart, ICartItem } from '../shared/models/cart';
import { CartService } from './cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent implements OnInit {
  cart$: Observable<ICart>;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cart$ = this.cartService.cart$;
  }

  removeItemFromCart(item: ICartItem) {
    this.cartService.removeItemFromCart(item);
  }
  incrementItemQuantity(item: ICartItem) {
    this.cartService.incrementItemQuantity(item);
  }
  decrementItemQuantity(item: ICartItem) {
    this.cartService.decrementItemQuantity(item);
  }
}

import { Component, OnInit } from '@angular/core';
import { CartService } from './cart/cart.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    // check for local storage
    //getItem return null if not exist

    const cartId = localStorage.getItem('cart_id');

    if (cartId) {
      this.cartService.getCart(cartId).subscribe(
        () => {
          console.log('cart initialized ');
        },
        (error) => {
          console.log(error);
        } 
      );
    }
  }
}

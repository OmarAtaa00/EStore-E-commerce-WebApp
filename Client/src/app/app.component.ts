import { Component, OnInit } from '@angular/core';
import { CartService } from './cart/cart.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(
    private cartService: CartService,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.loadCart();
    this.loadCurrentUser();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');

    this.accountService.loadCurrentUser(token).subscribe(
      () => {
        console.log('user logged ');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  loadCart() {
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

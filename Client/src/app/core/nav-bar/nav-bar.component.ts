import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ICart } from 'src/app/shared/models/cart';
import { CartService } from '../../cart/cart.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  cart$: Observable<ICart>;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cart$ = this.cartService.cart$;
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { ICart, ICartItem, Cart, ICartTotal } from '../shared/models/cart';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product';
import { isNgTemplate } from '@angular/compiler';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;

  /**
   * A variant of Subject that requires an initial value and emits its current
   * value whenever it is subscribed to.
   *
   * @class BehaviorSubject<T>
   *
   * BehaviorSubject is both observer and type of observable.
   * Every observer on subscribe gets current value.
     Current value is either latest value emitted by source observable using next() method or initial/default value.
   */
  private cartSource = new BehaviorSubject<ICart>(null);
  cart$ = this.cartSource.asObservable();

  private cartTotalSource = new BehaviorSubject<ICartTotal>(null);
  cartTotal$ = this.cartTotalSource.asObservable();

  constructor(private http: HttpClient) {}

  // when app restarts we call getCart and it will set the cart to whatever receive from the api

  getCart(id: string) {
    return this.http.get(this.baseUrl + 'cart?id=' + id).pipe(
      map((cart: any) => {
        this.cartSource.next(cart);
        this.calculateTotal();
      })
    );
  }

  setCart(cart: ICart) {
    return this.http.post(this.baseUrl + 'cart', cart).subscribe(
      (response: any) => {
        this.cartSource.next(response);
        this.calculateTotal();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getCurrentCartValue() {
    return this.cartSource.value;
  }

  addItemToCart(item: IProduct, quantity = 1) {
    const itemToAdd: ICartItem = this.mapProductItemToCartItem(item, quantity);
    const cart = this.getCurrentCartValue() ?? this.createCart();
    cart.items = this.addOrUpdateItem(cart.items, itemToAdd, quantity);
    //addOrUpdateItem to cover up if thy have similar item just update the quantity

    this.setCart(cart);
  }
  private addOrUpdateItem(
    items: ICartItem[],
    itemToAdd: ICartItem,
    quantity: number
  ): ICartItem[] {
    //if product.id matches with item.id then increase quantity
    const index = items.findIndex((i) => i.id === itemToAdd.id);

    if (index === -1) {
      // not found
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  createCart(): ICart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

  /**
   * @reduce
   * Calls the specified callback function for all the elements in an array.
   *  The return value of the callback function is the accumulated result,
   * and is provided as an argument in the next call to the callback function.
   */

  private calculateTotal() {
    //get the cart
    const cart = this.getCurrentCartValue();
    const shipping = 0;
    const subtotal = cart.items.reduce((a, b) => b.price * b.quantity + a, 0);
    const total = subtotal + shipping;

    // # set the next value of the observable
    this.cartTotalSource.next({ shipping, subtotal, total });
  }
  private mapProductItemToCartItem(
    item: IProduct,
    quantity: number
  ): ICartItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType,
    };
  }

  incrementItemQuantity(item: ICartItem) {
    //get the cart value
    const cart = this.getCurrentCartValue();

    //check if there an item of the kind
    const foundItemIndex = cart.items.findIndex((x) => x.id === item.id);
    //increment the item if found by
    cart.items[foundItemIndex].quantity++;

    //set the new cart
    this.setCart(cart);
  }
  decrementItemQuantity(item: ICartItem) {
    //get the cart value
    const cart = this.getCurrentCartValue();

    //check if there an item of the kind
    const foundItemIndex = cart.items.findIndex((x) => x.id === item.id);

    // check if the current quantity is greater the 1 then -- if 1 remove

    if (cart.items[foundItemIndex].quantity > 1) {
      cart.items[foundItemIndex].quantity--;
    } else {
      this.removeItemFromCart(item);
    }
  }
  removeItemFromCart(item: ICartItem) {
    //get the cart
    const cart = this.getCurrentCartValue();
    //check items from an id match using some() then //remove the item use filter to return items that doesn't match
    if (cart.items.some((x) => x.id === item.id)) {
      cart.items = cart.items.filter((x) => x.id !== item.id);
      //check for cart item length > 0 => set the cart otherwise delete the whole cart
      if (cart.items.length > 0) {
        this.setCart(cart);
      } else {
        this.deleteCart(cart);
      }
    }
  }
  deleteCart(cart: ICart) {
    //go to the API and remove it http.delete
    return this.http.delete(this.baseUrl + 'cart?id=' + cart.id).subscribe(
      () => {
        this.cartSource.next(null);
        this.cartTotalSource.next(null);
        localStorage.removeItem('cart_id');
      },
      (error) => {
        console.log(error);
      }
    );
  }
}

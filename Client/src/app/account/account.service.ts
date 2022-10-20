import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { IUser } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})

//token we want to persist in local storage
//if browser closed we can check if they had a token then log them in
export class AccountService {
  //apiUrl
  baseUrl = environment.apiUrl;
  //Behavior subject of type IUser init null
  private currentUserSource = new ReplaySubject<IUser>(1);
  //observable of currentUser
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private routes: Router) {}

  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }
    //pass the token to API, cuz we need to be authorized first, pass as header
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<IUser>(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  //login()
  login(values: any) {
    return this.http.post<IUser>(this.baseUrl + '/account/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }
  //register()
  register(values: any) {
    return this.http
      .post<IUser>(this.baseUrl + '/account/register', values)
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        })
      );
  }

  //logout()
  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.routes.navigateByUrl('/');
  }

  //checkEmailExist()
  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + '/account/emailexists?email= ' + email);
  }
}

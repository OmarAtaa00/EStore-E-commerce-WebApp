import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from '../../account/account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(private accountService: AccountService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> {
    //we don't need to subscribe to the observable cuz Router will auto sub
    return this.accountService.currentUser$.pipe(
      map((auth): any => {
        if (auth) {
          return true;
        }
        this.router.navigate(['account/login'], {
          queryParams: { returnUrl: state.url },
        });
      })
    );
  }
}

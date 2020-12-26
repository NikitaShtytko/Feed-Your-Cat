import { Injectable } from '@angular/core';
import {Router} from "@angular/router";
import {CookieService} from "../cookie/cookie.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public router: Router,
              public auth: CookieService) {
  }

  canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}

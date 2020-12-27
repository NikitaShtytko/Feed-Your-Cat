import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../../services/auth/auth.service";
import {CookieService} from "../../../services/cookie/cookie.service";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  // public loading = true;
  public loading = false;

  constructor(
    private cookieService: CookieService
  ) { }

  ngOnInit(): void {

  }

  logOut() {
    this.cookieService.deleteAuth();
  }
}

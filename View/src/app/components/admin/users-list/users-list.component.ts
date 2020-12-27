import {Component, OnInit} from '@angular/core';
import {User} from "../../../models/user";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";
import {CookieService} from "../../../services/cookie/cookie.service";

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  public user: User[];
  public loading = true;

  constructor(
    private dialog: MatDialog,
    private userService: UserService,
    private cookieService: CookieService
  ) {}

  public subscriptions: Subscription[] = [];

  openDialog() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      title: 'delete'
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    if (dialogConfig) {

    }

    dialogRef.afterClosed().subscribe(
      data => console.log("Dialog output:", data)
    );
  }

  ngOnInit(): void {
    this.subscriptions.push(this.userService.getUsers().subscribe(response => {
        this.user = response;
        this.loading = false;
    }));
  }

  logOut() {
    this.cookieService.deleteAuth();
  }
}

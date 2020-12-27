import {Component, OnInit} from '@angular/core';
import {Request} from "../../../models/request";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";
import {User} from "../../../models/user";
import {FeederService} from "../../../services/feeder/feeder.service";
import {Feeder} from "../../../models/feeder";
import {CookieService} from "../../../services/cookie/cookie.service";

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent implements OnInit {
  public user: User[];
  public feeder: Feeder[];

  public loadingUsers = true;
  public loadingFeeders = true;

  constructor(private dialog: MatDialog,
              private userService: UserService,
              private feederService: FeederService,
              private cookieService: CookieService) {
  }

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {

    this.subscriptions.push(this.userService.getModeration().subscribe(response => {
      this.user = response.data;
      this.loadingUsers = false;
    }));

    this.subscriptions.push(this.feederService.getModeration().subscribe(response => {
      this.feeder = response.data;
      this.loadingFeeders = false;
    }));
  }

  openDialog(title: string, type: string, id: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      title: title,
      id: id,
      type: type,
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    if (dialogConfig) {

    }

    dialogRef.afterClosed().subscribe(
      data => {
        if (data === true){
          this.ngOnInit();
        }
      }
    );
  }

  logOut() {
    this.cookieService.deleteAuth();
  }

}


import {Component, OnInit} from '@angular/core';
import {Request} from "../../../models/request";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";
import {User} from "../../../models/user";

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent implements OnInit {
  public request: Request;
  public user: User[];

  constructor(private dialog: MatDialog,
              private userService: UserService) {
  }

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {
    this.request = new Request();
    this.request.id = 123;

    this.subscriptions.push(this.userService.getModeration().subscribe(response => {
      this.user = response;
    }));
  }

  openDialog(title: string, id: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      title: title,
      userId: id,
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    if (dialogConfig) {

    }

    dialogRef.afterClosed().subscribe(
      data => console.log("Dialog output:", data)
    );
  }

}


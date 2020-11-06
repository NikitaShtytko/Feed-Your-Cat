import {Component, OnInit} from '@angular/core';
import {User} from "../../../models/user";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  public user: User;

  constructor(private dialog: MatDialog) {}

  openDialog() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      id: 1,
      title: 'Angular For Beginners'
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      data => console.log("Dialog output:", data)
    );
  }

  ngOnInit(): void {
    this.user = new User();
    this.user.id = 2;
    this.user.name = 'nick';
    this.user.email = 'nik@mail.ru';
    this.user.role = 'user';
  }
}

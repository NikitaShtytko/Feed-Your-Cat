import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";


@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  public data: any;
  public reverse = false;
  public userId = null;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data,
    private userService: UserService,
  ) {
    this.data = data;

    if (this.data.title == 'accept'){
      this.reverse = !this.reverse;
      this.userId = this.data.userId;
      console.log(this.userId);
    }
  }

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {
    console.log(this.data);
  }

  save() {
    this.subscriptions.push(this.userService.accept(this.userId).subscribe(response => {

    }));
    this.dialogRef.close(true);
  }

  close() {
    this.dialogRef.close(false);
  }
}

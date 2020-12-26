import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";
import {FeederService} from "../../../services/feeder/feeder.service";


@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  public data: any;
  public reverse = false;
  public id = null;
  public type = null;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data,
    private userService: UserService,
    private feederService: FeederService,
  ) {
    this.data = data;

    if (this.data.title == 'accept'){
      this.reverse = !this.reverse;
      this.id = this.data.id;
      this.type = this.data.type;
    }
    else {
      this.id = this.data.id;
      this.type = this.data.type;
    }
  }

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {
  }

  approve() {
    if (this.type === 'user'){
      this.subscriptions.push(this.userService.accept(this.id).subscribe(response => {
        this.dialogRef.close(true);
      }));
    }
    else {
      this.subscriptions.push(this.feederService.accept(this.id).subscribe(response => {
        this.dialogRef.close(true);
      }));
    }
  }

  notApprove() {
    if (this.type === 'user'){
      this.subscriptions.push(this.userService.decline(this.id).subscribe(response => {
        this.dialogRef.close(true);
      }));
    }
    else {
      this.subscriptions.push(this.feederService.decline(this.id).subscribe(response => {
        this.dialogRef.close(true);
      }));
    }
  }

  close() {
    this.dialogRef.close(false);
  }
}

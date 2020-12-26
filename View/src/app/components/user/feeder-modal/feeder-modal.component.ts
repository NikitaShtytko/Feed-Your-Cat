import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../../admin/confirm-dialog/confirm-dialog.component";
import {CookieService} from "../../../services/cookie/cookie.service";
import {Feeder} from "../../../models/feeder";
import {FeederService} from "../../../services/feeder/feeder.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-feeder-modal',
  templateUrl: './feeder-modal.component.html',
  styleUrls: ['./feeder-modal.component.css']
})
export class FeederModalComponent implements OnInit {
  public feeder: Feeder;
  public is_exist;
  public empty;
  time: any;

  public subscriptions: Subscription[] = [];

  constructor(
    private _authCookie: CookieService,
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    private feederService: FeederService,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.is_exist = data.title != 'request';

    console.log(data.data);

    if (this.is_exist) {
      this.feeder = data.data;
      switch (this.feeder.is_empty) {
        case false:
          this.empty = "Not empty";
          break;
        case true:
          this.empty = "Empty"
          break;
      }
    }
  }


  ngOnInit(): void {

  }

  fill() {
    this.subscriptions.push(this.feederService.fillTheFeeder(this.feeder.id).subscribe(response => {
      this.feeder.fullness = response;
    }));
  }

  feed() {
    this.subscriptions.push(this.feederService.feedTheCat(this.feeder.id).subscribe(response => {
      this.feeder.fullness = response;
    }));
  }

  close() {
    this.dialogRef.close(false);
  }
}

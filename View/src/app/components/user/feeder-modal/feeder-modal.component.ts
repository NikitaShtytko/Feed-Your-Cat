import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../../admin/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-feeder-modal',
  templateUrl: './feeder-modal.component.html',
  styleUrls: ['./feeder-modal.component.css']
})
export class FeederModalComponent implements OnInit {
  public data: any;
  public is_exist;
  time: any;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.is_exist = data.title != 'request';

    if (this.is_exist) {
      this.data = data;

      switch (this.data.empty) {
        case 0:
          this.data.empty = "Not empty";
          break;
        case 1:
          this.data.empty = "Empty"
          break;
      }
    }
  }


  ngOnInit(): void {

  }

  feed() {
    console.log(this.time);
  }

  close() {
    this.dialogRef.close(false);
  }

}

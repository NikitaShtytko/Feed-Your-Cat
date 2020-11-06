import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";


@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  public data: any;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.data = data;
  }

  ngOnInit(): void {
    console.log(this.data);
  }

  save() {
    this.dialogRef.close(true);
  }

  close() {
    this.dialogRef.close(false);
  }
}

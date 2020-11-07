import { Component, OnInit } from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../../admin/confirm-dialog/confirm-dialog.component";
import {FeederModalComponent} from "../feeder-modal/feeder-modal.component";

@Component({
  selector: 'app-feeders-page',
  templateUrl: './feeders-page.component.html',
  styleUrls: ['./feeders-page.component.css']
})
export class FeedersPageComponent implements OnInit {
// public loading = true;
  public loading = false;
  // public arr = [1,2,3,4,5,6,7,8, 9, 10, 11, 12, 13, 14, 15, 16];
  public arr = [1,2,3,4,5,6,7,8, 9, 10, 11, 12, 13, 14];

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {

  }

  openDialog(way) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      title: way
    };
    const dialogRef = this.dialog.open(FeederModalComponent, dialogConfig);

    if (dialogConfig){

    }

    dialogRef.afterClosed().subscribe(
      data => console.log("Dialog output:", data)
    );
  }

}

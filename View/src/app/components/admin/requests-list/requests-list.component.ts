import { Component, OnInit } from '@angular/core';
import {Request} from "../../../models/request";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent implements OnInit {
  public request: Request;

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {
    this.request = new Request();
    this.request.id = 123;
  }

  openDialog(title: string) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      title: title
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    if (dialogConfig){

    }

    dialogRef.afterClosed().subscribe(
      data => console.log("Dialog output:", data)
    );
  }

}


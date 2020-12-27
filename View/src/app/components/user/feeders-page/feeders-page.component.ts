import {Component, OnInit} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {FeederModalComponent} from "../feeder-modal/feeder-modal.component";
import {FeederService} from "../../../services/feeder/feeder.service";
import {Subscription} from "rxjs";
import {Feeder} from "../../../models/feeder";

@Component({
  selector: 'app-feeders-page',
  templateUrl: './feeders-page.component.html',
  styleUrls: ['./feeders-page.component.css']
})
export class FeedersPageComponent implements OnInit {
  public feeder: Feeder[];
  public loading = true;
  // public loading = false;

  constructor(private dialog: MatDialog,
              private feederService: FeederService) {}

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {
    this.subscriptions.push(this.feederService.feederList().subscribe(response => {
      this.feeder = response;
      console.log(this.feeder);
      this.loading = false;
    }));
  }

  openDialog(way) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = {
      data: way
    };
    const dialogRef = this.dialog.open(FeederModalComponent, dialogConfig);

    if (dialogConfig){

    }

    dialogRef.afterClosed().subscribe(
      data => {
        if (data === true){
          this.ngOnInit();
        }
        console.log("Dialog output:", data)
      }
    );
  }

}

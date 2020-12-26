import {Component, OnInit} from '@angular/core';
import {Request} from "../../../models/request";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../confirm-dialog/confirm-dialog.component";
import {UserService} from "../../../services/user/user.service";
import {Subscription} from "rxjs";
import {User} from "../../../models/user";
import {FeederService} from "../../../services/feeder/feeder.service";
import {Feeder} from "../../../models/feeder";

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent implements OnInit {
  public user: User[];
  public feeder: Feeder[];

  constructor(private dialog: MatDialog,
              private userService: UserService,
              private feederService: FeederService) {
  }

  public subscriptions: Subscription[] = [];

  ngOnInit(): void {

    this.subscriptions.push(this.userService.getModeration().subscribe(response => {
      this.user = response.data;
    }));

    this.subscriptions.push(this.feederService.getModeration().subscribe(response => {
      this.feeder = response.data;
    }));
  }

  openDialog(title: string, type: string, id: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    console.log(id);

    dialogConfig.data = {
      title: title,
      id: id,
      type: type,
    };

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    if (dialogConfig) {

    }

    dialogRef.afterClosed().subscribe(
      data => {
                                           console.log('data = ' + data);
                                             console.log('type = ' + type);

        if (type === 'user'){
                                            console.log('user');

          let index = this.user.findIndex(item => item.id == data);
          this.user = this.user.slice(index, 1);
        }
        else {
                                        console.log('feeder');

          let index = this.feeder.findIndex(item => item.id == data);

                                                console.log('index = ' + index);
                                                console.log(this.feeder);

          this.feeder = this.feeder.slice(index, 1);

                                                   console.log(this.feeder);
        }
      }
    );
  }

}


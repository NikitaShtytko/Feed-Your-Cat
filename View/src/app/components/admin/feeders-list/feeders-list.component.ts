import {Component, OnInit} from '@angular/core';
import {Feeder} from "../../../models/feeder";
import {Subscription} from "rxjs";
import {FeederService} from "../../../services/feeder/feeder.service";
import {User} from "../../../models/user";
import {CookieService} from "../../../services/cookie/cookie.service";
import {saveAs} from 'file-saver';


@Component({
  selector: 'app-feeders-list',
  templateUrl: './feeders-list.component.html',
  styleUrls: ['./feeders-list.component.css']
})
export class FeedersListComponent implements OnInit {
  public feeder: Feeder[];
  public loading = true;

  constructor(
    private feederService: FeederService,
    private cookieService: CookieService
  ) { }

  public subscriptions: Subscription[] = [];


  ngOnInit(): void {
    this.subscriptions.push(this.feederService.getFeedersAdmin().subscribe(response => {
      this.feeder = response;
      this.loading = false;
    }));
  }

  logOut() {
    this.cookieService.deleteAuth();
  }

  logs(id: number, name: string){
    this.subscriptions.push(this.feederService.logsAdmin(id).subscribe(response => {
      let blob = new Blob([response], {type: "text/plain;charset=utf-8"});
      saveAs(blob, "Feeder " + name + ".txt");
    }));
  }
}

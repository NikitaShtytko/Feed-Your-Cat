import {Component, OnInit} from '@angular/core';
import {Feeder} from "../../../models/feeder";
import {Subscription} from "rxjs";
import {FeederService} from "../../../services/feeder/feeder.service";
import {User} from "../../../models/user";


@Component({
  selector: 'app-feeders-list',
  templateUrl: './feeders-list.component.html',
  styleUrls: ['./feeders-list.component.css']
})
export class FeedersListComponent implements OnInit {
  public feeder: Feeder[];

  constructor(
    private feederService: FeederService
  ) { }

  public subscriptions: Subscription[] = [];


  ngOnInit(): void {
    this.subscriptions.push(this.feederService.getFeedersAdmin().subscribe(response => {
      this.feeder = response.data;
      console.log(this.feeder);
    }));
  }
}

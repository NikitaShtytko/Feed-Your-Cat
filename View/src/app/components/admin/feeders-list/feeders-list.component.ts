import {Component, OnInit} from '@angular/core';
import {Feeder} from "../../../models/feeder";


@Component({
  selector: 'app-feeders-list',
  templateUrl: './feeders-list.component.html',
  styleUrls: ['./feeders-list.component.css']
})
export class FeedersListComponent implements OnInit {
  public feeder: Feeder;

  constructor() { }

  ngOnInit(): void {
    this.feeder = new Feeder();
    this.feeder.id = 2;
    this.feeder.user_id = 'nick';
    this.feeder.type = 'qewq';
    this.feeder.empty = false;
    this.feeder.status = 80;
    this.feeder.log_id = 1;
  }
}

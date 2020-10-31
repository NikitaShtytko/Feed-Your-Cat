import { Component, OnInit } from '@angular/core';
import {Request} from "../../../models/request";

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent implements OnInit {
  public request: Request;

  constructor() { }

  ngOnInit(): void {
    this.request = new Request();
    this.request.id = 123;
  }

}


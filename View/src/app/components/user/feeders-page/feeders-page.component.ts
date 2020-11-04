import { Component, OnInit } from '@angular/core';

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

  constructor() { }

  ngOnInit(): void {

  }

}

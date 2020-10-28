import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found-page',
  templateUrl: './not-found-page.component.html',
  styleUrls: ['./not-found-page.component.css']
})
export class NotFoundPageComponent implements OnInit {

  constructor() { }

  public random;

  ngOnInit(): void {
    this.getRandomInt(4);
  }

    getRandomInt(max) {
    this.random =  Math.floor(Math.random() * Math.floor(max)) + 1;
    console.log(this.random);
  }

}

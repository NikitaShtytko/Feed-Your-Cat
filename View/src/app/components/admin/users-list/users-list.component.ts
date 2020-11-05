import {Component, OnInit} from '@angular/core';
import {User} from "../../../models/user";

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  public user: User;

  constructor() {
  }

  ngOnInit(): void {
    this.user = new User();
    this.user.id = 2;
    this.user.name = 'nick';
    this.user.email = 'nik@mail.ru';
    this.user.role = 'user';
  }

}

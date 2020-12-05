import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from "rxjs";
import {User} from "../../models/user";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getUsers(): Observable<User[]>{
    return this.httpClient.get<User[]>('/api/users');
  }

  register(user: User): Observable<User[]>{
    return this.httpClient.get<User[]>('/api/users/register');
  }

  login(user: User): Observable<User[]>{
    return this.httpClient.get<User[]>('/api/users/login');
  }
}

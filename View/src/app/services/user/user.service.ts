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
    return this.httpClient.get<User[]>('http://localhost:5000/api/users');
  }

  getModeration(): Observable<User[]>{
    return this.httpClient.get<User[]>('http://localhost:5000/api/users/moderation');
  }

  accept(id: number): Observable<User>{
    return this.httpClient.get<User>('http://localhost:5000/api/users/accept/' + id);
  }

  decline(id: number): Observable<User>{
    return this.httpClient.delete<User>('http://localhost:5000/api/users/' + id);
  }

  register(user: User): Observable<User[]>{
    return this.httpClient.post<User[]>('http://localhost:5000/api/users/register', user);
  }

  login(user: { login: string, password: string }): Observable<User[]>{
    return this.httpClient.post<User[]>('http://localhost:5000/api/users/login', user);
  }

  existEmail(email: string): Observable<User>{
    return this.httpClient.get<User>('http://localhost:5000/api/users/email' + email);
  }
}

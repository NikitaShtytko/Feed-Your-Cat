import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from "rxjs";
import {User} from "../../models/user";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  route = 'http://localhost:5000';
  constructor(private httpClient: HttpClient) { }

  getUsers(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/moderated/users');
  }

  getModeration(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/moderation/users');
  }

  accept(id: number): Observable<User>{
    return this.httpClient.get<User>(this.route + '/api/admin/users/approve/' + id);
  }

  decline(id: number): Observable<User>{
    return this.httpClient.delete<User>(this.route + '/api/admin/users/decline/' + id);
  }

  register(user: User): Observable<User[]>{
    return this.httpClient.post<User[]>(this.route + '/api/auth/sign-up', user);
  }

  login(user: { email: string, password: string }): Observable<User[]>{
    return this.httpClient.post<User[]>(this.route + '/api/auth/sign-in', user);
  }

  existEmail(email: string): Observable<boolean>{
    return this.httpClient.get<boolean>(this.route + '/api/email?email=' + email);
  }

  logs(id: number): Observable<string>{
    return this.httpClient.get<string>(this.route + '/api/admin/users/log/' + id);
  }
}

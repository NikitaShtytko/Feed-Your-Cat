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
    return this.httpClient.get<User[]>('http://localhost:8000/api/admin/users');
  }

  getModeration(): Observable<User[]>{
    return this.httpClient.get<User[]>('http://localhost:8000/api/admin/users/moderation');
  }

  accept(id: number): Observable<User>{
    return this.httpClient.get<User>('http://localhost:8000/api/admin/user/approve/' + id);
  }

  decline(id: number): Observable<User>{
    return this.httpClient.delete<User>('http://localhost:8000/api/user/not-approve/' + id);
  }

  register(user: User): Observable<User[]>{
    return this.httpClient.post<User[]>('http://localhost:8000/api/auth/sign-up', user);
  }

  login(user: { email: string, password: string }): Observable<User[]>{
    return this.httpClient.post<User[]>('http://localhost:8000/api/auth/sign-in', user);
  }

  existEmail(email: string): Observable<boolean>{
    return this.httpClient.get<boolean>('http://localhost:8000/api/email?email=' + email);
  }
}

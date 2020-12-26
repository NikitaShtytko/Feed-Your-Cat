import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Feeder} from "../../models/feeder";

@Injectable({
  providedIn: 'root'
})
export class FeederService {

  route = 'http://localhost:8000';
  constructor(private httpClient: HttpClient) { }

  getFeedersAdmin(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/feeders');
  }

  getModeration(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/feeders/moderation');
  }

  accept(id: number): Observable<Feeder>{
    return this.httpClient.get<Feeder>(this.route + '/api/admin/feeders/approve/' + id);
  }

  decline(id: number): Observable<Feeder>{
    return this.httpClient.delete<Feeder>(this.route + '/api/feeders/not-approve/' + id);
  }
}

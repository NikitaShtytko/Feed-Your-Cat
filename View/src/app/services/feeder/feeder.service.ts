import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Feeder} from "../../models/feeder";

@Injectable({
  providedIn: 'root'
})
export class FeederService {

  route = 'http://localhost:5000';
  constructor(private httpClient: HttpClient) { }

  getFeedersAdmin(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/moderated/feeders');
  }

  getModeration(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/admin/moderation/feeders');
  }

  accept(id: number): Observable<Feeder>{
    return this.httpClient.get<Feeder>(this.route + '/api/admin/feeders/approve/' + id);
  }

  decline(id: number): Observable<Feeder>{
    return this.httpClient.delete<Feeder>(this.route + '/api/admin/feeders/decline/' + id);
  }

  feederList(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/user/feeders');
  }

  feedTheCat(id: number): Observable<number>{
    return this.httpClient.get<number>(this.route + '/api/user/feeders/feed/' + id);
  }

  fillTheFeeder(id: number): Observable<number>{
    return this.httpClient.get<number>(this.route + '/api/user/feeders/fill/' + id);
  }

  newFeeder(feeder: Feeder): Observable<Feeder>{
    return this.httpClient.post<Feeder>(this.route + '/api/user/feeders/register', feeder);
  }

  newTag(tag: any): Observable<Feeder> {
    return this.httpClient.put<Feeder>(this.route + '/api/feeders', tag);
  }

  deleteTag(id: number): Observable<Feeder> {
    return this.httpClient.delete<Feeder>(this.route + '/api/feeders' + id);
  }
}

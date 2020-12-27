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

  feederList(): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/feeders');
  }

  feedTheCat(id: number): Observable<number>{
    return this.httpClient.get<number>(this.route + '/api/feeders/feed/' + id);
  }

  fillTheFeeder(id: number): Observable<number>{
    return this.httpClient.get<number>(this.route + '/api/feeders/fill/' + id);
  }

  newFeeder(feeder: Feeder): Observable<Feeder>{
    return this.httpClient.post<Feeder>(this.route + '/api/feeders', feeder);
  }









  newTag(tag: any): Observable<Feeder> {
    return this.httpClient.put<Feeder>(this.route + '/api/feeders', tag);
  }

  deleteTag(id: number): Observable<Feeder> {
    return this.httpClient.delete<Feeder>(this.route + '/api/feeders' + id);
  }

  newSchedule(schedule: { id: number; time: any }): Observable<Feeder>  {
    return this.httpClient.put<Feeder>(this.route + '/api/feeders', schedule);
  }

  deleteSchedule(id: number): Observable<Feeder> {
    return this.httpClient.delete<Feeder>(this.route + '/api/feeders' + id);
  }
}

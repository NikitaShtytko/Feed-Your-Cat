import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Feeder} from "../../models/feeder";
import {Tag} from "../../models/tag";
import {Schedule} from "../../models/schedule";

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

  TagList(id: number): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(this.route + '/api/user/feeders/tags/' + id);
  }

  newTag(tag: any): Observable<Tag[]> {
    return this.httpClient.put<Tag[]>(this.route + '/api/user/feeders/tag', tag);
  }

  deleteTag(id: number): Observable<Tag[]> {
    return this.httpClient.delete<Tag[]>(this.route + '/api/user/feeders/tag/' + id);
  }

  newSchedule(schedule: { id: number; time: any }): Observable<Schedule[]>  {
    return this.httpClient.put<Schedule[]>(this.route + '/api/user/feeders/schedule', schedule);
  }

  deleteSchedule(id: number): Observable<Schedule[]> {
    return this.httpClient.delete<Schedule[]>(this.route + '/api/user/feeders/schedule/' + id);
  }

  logs(id: number): Observable<any>{
    return this.httpClient.get<any>(this.route + '/api/user/feeders/log/id');
  }
}

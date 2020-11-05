import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {MDBBootstrapModule} from 'angular-bootstrap-md';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {LoginPageComponent} from './components/login-page/login-page.component';
import {RegisterPageComponent} from './components/register-page/register-page.component';
import {NotFoundPageComponent} from './components/not-found-page/not-found-page.component';
import {RouterModule, Routes} from "@angular/router";
import {NoopAnimationsModule} from '@angular/platform-browser/animations';
import {UsersListComponent} from './components/admin/users-list/users-list.component';
import {FeedersListComponent} from './components/admin/feeders-list/feeders-list.component';
import {RequestsListComponent} from './components/admin/requests-list/requests-list.component';
import {AdminPanelComponent} from "./components/admin/admin-panel/admin-panel.component";
import {FeedersPageComponent} from './components/user/feeders-page/feeders-page.component';

const appRoutes: Routes = [
  {path: 'login', component: LoginPageComponent},
  {path: 'register', component: RegisterPageComponent},
  {path: 'admin', component: AdminPanelComponent},
  {path: 'admin/users', component: UsersListComponent},
  {path: 'admin/feeders', component: FeedersListComponent},
  {path: 'admin/requests', component: RequestsListComponent},
  {path: '', component: FeedersPageComponent},

  {path: '**', component: NotFoundPageComponent},
];

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    RegisterPageComponent,
    NotFoundPageComponent,
    AdminPanelComponent,
    UsersListComponent,
    FeedersListComponent,
    RequestsListComponent,
    FeedersPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(appRoutes),
    MDBBootstrapModule.forRoot(),
    NoopAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}

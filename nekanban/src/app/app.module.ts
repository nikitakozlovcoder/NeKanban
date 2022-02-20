import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {MaterialExampleModule} from '../material.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatNativeDateModule} from '@angular/material/core';
import {HttpClientModule} from '@angular/common/http';


import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UserService } from './services/user.service';
import { AuthorizationComponent } from './authorization/authorization.component';
import {RouterModule, Routes} from "@angular/router";
import { DialogComponent } from './dialog/dialog.component';
import { DeskComponent } from './desk/desk.component';
import {DeskService} from "./services/desk.service";
import {BaseHttpService} from "./services/base_http.service";
import {DeskGuard} from "./guards/desk.guard";
import { DeskCreationComponent } from './desk-creation/desk-creation.component';
import { DeskCreationDialogComponent } from './desk-creation-dialog/desk-creation-dialog.component';
import { TaskCreationComponent } from './task-creation/task-creation.component';
import { ColumnCreationComponent } from './column-creation/column-creation.component';
import {ColumnService} from "./services/column.service";

const appRoutes: Routes =[
  { path: 'authorization', component: AuthorizationComponent},
  { path: '', component: DeskComponent, canActivate: [DeskGuard]}
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    AuthorizationComponent,
    DialogComponent,
    DeskComponent,
    DeskCreationComponent,
    DeskCreationDialogComponent,
    TaskCreationComponent,
    ColumnCreationComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MatNativeDateModule,
    MaterialExampleModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [UserService,
  DeskService,
  BaseHttpService,
  DeskGuard,
    DeskComponent,
  ColumnService],
  bootstrap: [AppComponent]
})
export class AppModule { }

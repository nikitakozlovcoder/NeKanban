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
import { TaskCreationComponent } from './task-creation/task-creation.component';
import { ColumnCreationComponent } from './column-creation/column-creation.component';
import {ColumnService} from "./services/column.service";
import { InviteComponent } from './invite/invite.component';
import { TodoCreationComponent } from './todo-creation/todo-creation.component';
import {TodoService} from "./services/todo.service";
import { TodoEditingComponent } from './todo-editing/todo-editing.component';
import { ColumnUpdatingComponent } from './column-updating/column-updating.component';
import {RolesService} from "./services/roles.service";
import {DeskUserService} from "./services/deskUser.service";

const appRoutes: Routes =[
  { path: 'authorization', component: AuthorizationComponent},
  { path: '', component: DeskComponent, canActivate: [DeskGuard]},
  { path: 'invite', component: InviteComponent}
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
    TaskCreationComponent,
    ColumnCreationComponent,
    InviteComponent,
    TodoCreationComponent,
    TodoEditingComponent,
    ColumnUpdatingComponent,
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
  ColumnService,
    TodoService,
  RolesService,
  DeskUserService],
  bootstrap: [AppComponent]
})
export class AppModule { }

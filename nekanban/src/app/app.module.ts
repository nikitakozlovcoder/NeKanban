import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialExampleModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { UserService } from './services/user.service';
import { AuthorizationComponent } from './components/auth/authorization/authorization.component';
import { RouterModule, Routes } from "@angular/router";
import { DialogComponent } from './dialog/dialog.component';
import { DeskComponent } from './components/desk/desk-show/desk.component';
import { DeskService } from "./services/desk.service";
import { BaseHttpService } from "./services/baseHttp.service";
import { DeskGuard } from "./guards/desk.guard";
import { DeskCreationComponent } from './components/desk/desk-creation/desk-creation.component';
import { TodoShowComponent } from './components/todo/todo-show/todo-show.component';
import { ColumnCreationComponent } from './components/column/column-creation/column-creation.component';
import { ColumnService } from "./services/column.service";
import { InviteComponent } from './components/invite/invite.component';
import { TodoCreationComponent } from './components/todo/todo-creation/todo-creation.component';
import {TodoService} from "./services/todo.service";
import { TodoEditingComponent } from './components/todo/todo-editing/todo-editing.component';
import { ColumnUpdatingComponent } from './components/column/column-updating/column-updating.component';
import { RolesService } from "./services/roles.service";
import { DeskUserService } from "./services/deskUser.service";
import {DataGeneratorService} from "./services/dataGenerator.service";
import {NgScrollbarModule} from "ngx-scrollbar";
import {CommentsService} from "./services/comments.service";

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
    TodoShowComponent,
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
        RouterModule.forRoot(appRoutes),
        NgScrollbarModule
    ],
  providers: [UserService,
  DeskService,
  BaseHttpService,
  DeskGuard,
    DeskComponent,
  ColumnService,
    TodoService,
  RolesService,
  DeskUserService,
  DataGeneratorService,
    CommentsService],
  bootstrap: [AppComponent]
})
export class AppModule { }

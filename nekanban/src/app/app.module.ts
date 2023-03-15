import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialExampleModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { UserService } from './services/user.service';
import { AuthorizationComponent } from './components/auth/authorization/authorization.component';
import { RouterModule, Routes } from "@angular/router";
import { DialogComponent } from './dialog/dialog.component';
import { DeskComponent } from './components/desk/desk-show/desk.component';
import { DeskService } from "./services/desk.service";
import { AppHttpService } from "./services/app-http.service";
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
import {UserStorageService} from "./services/userStorage.service";
import { SettingsComponent } from './components/settings/settings/settings.component';
import { HeaderComponent } from './components/partials/header/header.component';
import { GeneralSettingsComponent } from './components/settings/general-settings/general-settings.component';
import { UsersSettingsComponent } from './components/settings/users-settings/users-settings.component';
import { RolesSettingsComponent } from './components/settings/roles-settings/roles-settings.component';
import {MatListModule} from "@angular/material/list";
import { RoleCreationComponent } from './components/settings/dialogs/role-creation/role-creation.component';
import { RoleUpdatingComponent } from './components/settings/dialogs/role-updating/role-updating.component';
import { ConfirmationComponent } from './components/dialogs/confirmation/confirmation.component';
import { PageNotFoundComponent } from './components/routing/page-not-found/page-not-found.component';
import { SidenavComponent } from './components/partials/sidenav/sidenav.component';
import {TranslateLoader, TranslateModule, TranslateService} from "@ngx-translate/core";
import {TranslateHttpLoader} from "@ngx-translate/http-loader";
import {AuthInterceptor} from "./interceptors/auth/auth.interceptor";
import {EditorComponent, TINYMCE_SCRIPT_SRC} from "@tinymce/tinymce-angular";
import { SafeHtmlPipe } from './pipes/safehtml/safe-html.pipe';
import {EditorConfigService} from "./services/editor-config-service";

const appRoutes: Routes = [
  { path: 'authorization', component: AuthorizationComponent},
  { path: '', component: DeskComponent, canActivate: [DeskGuard]},
  { path: 'invite', component: InviteComponent},
  { path: 'desks/:id', component: DeskComponent, canActivate: [DeskGuard]},
  { path: 'desks/:id/settings', component: SettingsComponent, canActivate: [DeskGuard]},
  { path: 'desks', redirectTo: ''},
  { path: '**', component: PageNotFoundComponent}
];

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

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
    SettingsComponent,
    HeaderComponent,
    GeneralSettingsComponent,
    UsersSettingsComponent,
    RolesSettingsComponent,
    RoleCreationComponent,
    RoleUpdatingComponent,
    ConfirmationComponent,
    PageNotFoundComponent,
    SidenavComponent,
    SafeHtmlPipe,
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
        NgScrollbarModule,
        MatListModule,
        TranslateModule.forRoot({
            defaultLanguage: 'ru',
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        EditorComponent
    ],
  providers: [
    UserService,
    DeskService,
    AppHttpService,
    DeskGuard,
    DeskComponent,
    ColumnService,
    TodoService,
    RolesService,
    DeskUserService,
    DataGeneratorService,
    CommentsService,
    UserStorageService,
    TranslateService,
    EditorConfigService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    //{ provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

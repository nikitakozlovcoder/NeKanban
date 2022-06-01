import { Component } from '@angular/core';
import {Routes} from "@angular/router";
import {AuthorizationComponent} from "./authorization/authorization.component";
import {LoginComponent} from "./login/login.component";



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'nekanban';
  constructor() {
  }
}

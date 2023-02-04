import {Component, OnInit} from '@angular/core';
import {RolesService} from "./services/roles.service";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'nekanban';
  constructor(private readonly rolesService: RolesService) {
  }

  ngOnInit(): void {
    this.rolesService.initRoles();
  }
}

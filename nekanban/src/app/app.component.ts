import {Component, OnInit} from '@angular/core';
import {RolesService} from "./services/roles.service";
import {TranslateService} from "@ngx-translate/core";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'nekanban';
  constructor(private readonly rolesService: RolesService,
              private readonly translateService: TranslateService) {
  }

  ngOnInit(): void {
    this.rolesService.initPermissions();
    this.translateService.use('ru');
  }
}

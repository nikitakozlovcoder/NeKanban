import {Component, Input, OnInit} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskUser} from "../../../models/deskUser";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {UserService} from "../../../services/user.service";
import {DeskUserService} from "../../../services/deskUser.service";

@Component({
  selector: 'app-desk-info',
  templateUrl: './desk-info.component.html',
  styleUrls: ['./desk-info.component.css']
})
export class DeskInfoComponent implements OnInit {

  @Input() desk?: Desk;
  @Input() settingsLink?: any[];
  @Input() returnLink?: any[];
  @Input() currentUser?: DeskUser;
  @Input() roles: Role[] = [];
  constructor(public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService) { }

  ngOnInit(): void {
  }

  getDeskOwner() : DeskUser | undefined  {
    return this.desk?.deskUsers.find(el => el.isOwner);
  }

}

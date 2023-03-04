import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../models/desk";
import {Role} from "../../../models/Role";
import {DeskUser} from "../../../models/deskUser";
import {RolesService} from "../../../services/roles.service";
import {DeskUserService} from "../../../services/deskUser.service";
import {DeskService} from "../../../services/desk.service";

@Component({
  selector: 'app-users-settings',
  templateUrl: './users-settings.component.html',
  styleUrls: ['./users-settings.component.css']
})
export class UsersSettingsComponent implements OnInit {

  @Input() desk: Desk | undefined;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Input() desks: Desk[] = [];
  isUserRemoveLoaded = true;

  constructor(private readonly rolesService: RolesService,
              private readonly deskUserService: DeskUserService,
              private readonly deskService: DeskService) { }

  ngOnInit(): void {
  }

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.desk!.id);
  }

  changeUserRole(event: Event, deskUserId: number) {
    this.deskUserService.changeRole(deskUserId, parseInt((event.target as HTMLInputElement).value)).subscribe({
      next: (data: DeskUser[]) => {
        this.desk!.deskUsers = data.sort(function (a: DeskUser, b: DeskUser) {
          if (a.id > b.id) {
            return 1;
          }
          if (a.id < b.id) {
            return -1;
          }
          return 0;
        });
      },
      error: () => {
      }
    });
  }

  removeUser(usersId: number[]) {
    this.isUserRemoveLoaded = false;
    this.deskService.removeUserFromDesk(usersId, this.desk!.id).subscribe({
      next: data => {
        this.isUserRemoveLoaded = true;
        this.desk = data;
      },
      error: () => {
      }
    })
  }
}

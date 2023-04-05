import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../models/desk";
import {Role} from "../../../models/Role";
import {DeskUser} from "../../../models/deskUser";
import {RolesService} from "../../../services/roles.service";
import {DeskUserService} from "../../../services/deskUser.service";
import {DeskService} from "../../../services/desk.service";
import {Router} from "@angular/router";
import {ConfirmationComponent} from "../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {MatDialog} from "@angular/material/dialog";
import {BehaviorSubject} from "rxjs";

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
  isUserRemoveLoaded = new BehaviorSubject(true);

  constructor(public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly deskService: DeskService,
              private readonly router: Router,
              private readonly dialog: MatDialog) { }

  ngOnInit(): void {
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
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result == DialogActionTypes.Reject) {
        return;
      }
      this.makeRemoval(usersId);
    });

  }

  private makeRemoval(usersId: number[]) {
    this.isUserRemoveLoaded.next(false);
    this.deskService.removeUserFromDesk(usersId, this.desk!.id).subscribe({
      next: data => {
        if (usersId.some(el => el === this.deskUserService.getCurrentDeskUser(this.desk)!.user.id)) {
          this.router.navigate(['']).then();
        }
        this.desk = data;
      },
      error: () => {
      }
    }).add(() => {
      this.isUserRemoveLoaded.next(true);
    })
  }
}

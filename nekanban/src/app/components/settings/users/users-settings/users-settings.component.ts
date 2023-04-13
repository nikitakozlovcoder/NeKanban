import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../../models/desk";
import {Role} from "../../../../models/Role";
import {DeskUser} from "../../../../models/deskUser";
import {RolesService} from "../../../../services/roles.service";
import {DeskUserService} from "../../../../services/deskUser.service";
import {DeskService} from "../../../../services/desk.service";
import {Router} from "@angular/router";
import {ConfirmationComponent} from "../../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../../constants/DialogActionTypes";
import {MatDialog} from "@angular/material/dialog";
import {BehaviorSubject, switchMap} from "rxjs";
import {DeletionReason} from "../../../../constants/deletionReason";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";

@Component({
  selector: 'app-users-settings',
  templateUrl: './users-settings.component.html',
  styleUrls: ['./users-settings.component.css']
})
export class UsersSettingsComponent implements OnInit {

  @Input() desk: Desk | undefined;
  @Input() deskId?: number;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Input() desks: Desk[] = [];
  @Input() deletedUsers: DeskUser[] = [];
  isUserRemoveLoaded = new BehaviorSubject(true);
  areUsersLoaded = new BehaviorSubject(false);
  deletionReason = DeletionReason;
  hideTableHeaders = false;

  constructor(public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly deskService: DeskService,
              private readonly router: Router,
              private readonly dialog: MatDialog,
              private readonly breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe(result => {
        this.hideTableHeaders = result.matches;
      })
    this.loadDeletedUsers(this.deskId!);
  }

  changeUserRole(roleId: number, deskUserId: number) {
    this.deskUserService.changeRole(deskUserId, roleId).subscribe({
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

  loadDeletedUsers(deskId: number) {
    this.areUsersLoaded.next(false);
    this.deskUserService.getDeletedUsers(deskId).subscribe(result => {
      this.deletedUsers = result;
      this.areUsersLoaded.next(true);
    })
  }

  handleDeskChange(desk: Desk) {
    this.desk = desk;
    this.deskChange.emit(this.desk);
    this.loadDeletedUsers(this.desk.id);
  }

  handleDeskUsersChange(deskUsers: DeskUser[]) {
    this.desk!.deskUsers = deskUsers;
    this.deskChange.emit(this.desk);
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

  revertDeleted(deskUserId: number) {
    this.deskUserService.revertDeletedUser(deskUserId).pipe(switchMap(() => {
      return this.deskService.getDesk(this.desk!.id)
    })).subscribe(result => {
      this.desk = result;
      this.deskChange.emit(this.desk);
      this.loadDeletedUsers(this.desk.id);
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
        this.deskChange.emit(this.desk);
        this.loadDeletedUsers(this.desk.id);
      },
      error: () => {
      }
    }).add(() => {
      this.isUserRemoveLoaded.next(true);
    })
  }
}

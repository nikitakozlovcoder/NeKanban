import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {DeskUser} from "../../../../models/deskUser";
import {RolesService} from "../../../../services/roles.service";
import {Role} from "../../../../models/Role";
import {DeskUserService} from "../../../../services/deskUser.service";
import {BehaviorSubject, filter, switchMap} from "rxjs";
import {DeletionReason} from "../../../../constants/deletionReason";
import {ConfirmationComponent} from "../../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../../constants/DialogActionTypes";
import {MatDialog} from "@angular/material/dialog";
import {DeskService} from "../../../../services/desk.service";
import {Desk} from "../../../../models/desk";
import {Router} from "@angular/router";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";

@Component({
  selector: 'app-single-user',
  templateUrl: './single-user.component.html',
  styleUrls: ['./single-user.component.css']
})
export class SingleUserComponent implements OnInit {

  @Input() deskUser?: DeskUser;
  @Input() roles: Role[] = [];
  @Input() deskId?: number;
  @Input() currentDeskUser? : DeskUser;
  @Input() isUserDeleted?: boolean;
  @Output() deskChange = new EventEmitter<Desk>;
  @Output() deskUsersChange = new EventEmitter<DeskUser[]>;
  @Output() deletedUsersChange = new EventEmitter<DeskUser[]>;
  isUserRemoveLoaded = new BehaviorSubject(true);
  deletionReason = DeletionReason;
  showUserCard = false;
  constructor(public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly dialog: MatDialog,
              private readonly deskService: DeskService,
              private readonly router: Router,
              private readonly breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe(result => {
        this.showUserCard = result.matches;
      })
  }

  changeUserRole(roleId: number, deskUserId: number) {
    this.deskUserService.changeRole(deskUserId, roleId).subscribe({
      next: (data: DeskUser[]) => {
        this.deskUsersChange.emit(data.sort(function (a: DeskUser, b: DeskUser) {
          if (a.id > b.id) {
            return 1;
          }
          if (a.id < b.id) {
            return -1;
          }
          return 0;
        }));
      },
      error: () => {
      }
    });
  }

  removeUser(usersId: number[]) {
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().pipe(filter(x => x === DialogActionTypes.Accept))
      .subscribe(() => this.makeRemoval(usersId));

  }

  revertDeleted(deskUserId: number) {
    this.isUserRemoveLoaded.next(false);
    this.deskUserService.revertDeletedUser(deskUserId).pipe(switchMap(() => {
      return this.deskService.getDesk(this.deskId!)
    })).subscribe(result => {
      this.deskChange.emit(result);
    }).add(() => {
      this.isUserRemoveLoaded.next(true);
    });
  }

  private makeRemoval(usersId: number[]) {
    this.isUserRemoveLoaded.next(false);
    this.deskUserService.removeUserFromDesk(usersId, this.deskId!).subscribe({
      next: data => {
        if (usersId.some(el => el === this.currentDeskUser!.user.id)) {
          this.router.navigate(['']).then();
        }
        this.deskChange.emit(data);
      },
      error: () => {
      }
    }).add(() => {
      this.isUserRemoveLoaded.next(true);
    })
  }
}

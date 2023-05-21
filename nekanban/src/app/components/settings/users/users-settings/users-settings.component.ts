import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../../models/desk";
import {Role} from "../../../../models/Role";
import {DeskUser} from "../../../../models/deskUser";
import {RolesService} from "../../../../services/roles.service";
import {DeskUserService} from "../../../../services/deskUser.service";
import {BehaviorSubject, Subscription, switchMap} from "rxjs";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {UntilDestroy} from "@ngneat/until-destroy";

@UntilDestroy({checkProperties:true})
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
  hideTableHeaders = false;
  private subscriptions = new Subscription();

  constructor(public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.subscriptions.add(this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe(result => {
        this.hideTableHeaders = result.matches;
      }));
    this.loadDeletedUsers(this.deskId!);
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
}

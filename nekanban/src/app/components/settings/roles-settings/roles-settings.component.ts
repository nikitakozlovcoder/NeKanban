import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Desk } from "../../../models/desk";
import { Role } from "../../../models/Role";
import { Permission } from "../../../models/permission";
import { MatDialog } from "@angular/material/dialog";
import { RoleCreationComponent } from "../dialogs/role-creation/role-creation.component";
import { RoleUpdatingComponent } from "../dialogs/role-updating/role-updating.component";
import { ConfirmationComponent } from "../../dialogs/confirmation/confirmation.component";
import { DialogActionTypes } from "../../../constants/DialogActionTypes";
import { RolesService } from "../../../services/roles.service";
import { HttpErrorResponse } from "@angular/common/http";
import { TranslateService } from "@ngx-translate/core";
import {DialogService} from "../../../services/dialog.service";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {BehaviorSubject, filter, Subscription, switchMap, tap} from "rxjs";
import {UntilDestroy} from "@ngneat/until-destroy";

@UntilDestroy({checkProperties: true})
@Component({
  selector: 'app-roles-settings',
  templateUrl: './roles-settings.component.html',
  styleUrls: ['./roles-settings.component.css']
})
export class RolesSettingsComponent implements OnInit, OnChanges {

  @Input() desk?: Desk;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Output() rolesChange = new EventEmitter<Role[]>;
  @Input() desks: Desk[] = [];
  permissions: Permission[] = [];
  allPermissions: Permission[] = [];
  currentRole?: Role;
  showAccordion = false;
  defaultRoleLoaded = new BehaviorSubject(true);
  roleDeletionLoaded : BehaviorSubject<boolean>[] = [];
  private subscriptions = new Subscription();

  constructor(private readonly dialog: MatDialog,
              private readonly rolesService: RolesService,
              public readonly translate: TranslateService,
              private readonly dialogService: DialogService,
              private readonly breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.subscriptions.add(this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe(result => {
        this.showAccordion = result.matches;
      }));
    this.setLoadingStates();
  }

  ngOnChanges() {
    this.currentRole ||= this.roles[0];
    this.allPermissions = this.rolesService.permissions;
  }

  getPermissionsForRole(role: Role) {
    this.currentRole = role;
  }

  openRoleCreationDialog() {
    const dialogRef = this.dialog.open(RoleCreationComponent, {
      data: {deskId: this.desk?.id},
      width: '400px',
    });
    dialogRef.afterClosed().pipe(filter(x => x)).subscribe( result => {
      this.roles = result;
      this.rolesChange.emit(this.roles);
    }).add(() => this.setLoadingStates());
  }

  openRoleUpdatingDialog(role: Role, event: MouseEvent) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(RoleUpdatingComponent, {
      data: {role},
      width: '400px',
    });

    dialogRef.afterClosed().pipe(filter(x => x)).subscribe( result => {
      this.roles = result;
      this.rolesChange.emit(this.roles);
      this.updateCurrentRole();
    }).add(() => this.setLoadingStates());
  }

  openRoleDeletingDialog(role: Role, $event: MouseEvent) {
    $event.stopPropagation();
    const dialogRef = this.dialog.open(ConfirmationComponent);
    dialogRef.afterClosed().pipe(filter(x => x === DialogActionTypes.Accept),
      tap(() => this.roleDeletionLoaded[this.roles.findIndex(el => el.id === role.id)].next(false)),
      switchMap(() => this.rolesService.deleteRole(role.id))).subscribe({
      next: (data) => {
        this.roles = data;
        this.rolesChange.emit(this.roles);
        if (this.currentRole?.id == role.id) {
          this.currentRole = this.roles.find(x => x.isDefault);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.dialogService.openToast(error.error);
      }
    }).add(() => this.setLoadingStates());
  }

  currentRoleHasPermission(permission: Permission) {
    return this.currentRole?.permissions.some(el => el.permission === permission.permission);
  }

  roleHasPermission(role: Role, permission: Permission) {
    return role.permissions.some(el => el.permission === permission.permission);
  }

  grantPermissionToRole(role: Role, permission: Permission) {
    this.rolesService.grantPermission(role.id, permission.permission).subscribe(() => {
      role.permissions.push(permission);
    })
  }

  revokePermissionFromRole(permission: Permission) {
    this.rolesService.revokePermission(this.currentRole!.id, permission.permission).subscribe(() => {
      this.currentRole!.permissions = this.currentRole!.permissions.filter(el => {
        return el.permission != permission.permission;
      });
    })
  }

  setRoleAsDefault(role: Role) {
    this.defaultRoleLoaded.next(false);
    this.rolesService.setAsDefault(role.id).subscribe(result => {
      this.roles = result;
      this.rolesChange.emit(this.roles);
      this.updateCurrentRole();
    }).add(() => {
      this.defaultRoleLoaded.next(true);
    })
  }

  private updateCurrentRole() {
    this.currentRole = this.roles.find(el => el.id === this.currentRole!.id);
  }

  private setLoadingStates() {
    this.roleDeletionLoaded = [];
    this.roles.forEach(() => this.roleDeletionLoaded.push(new BehaviorSubject<boolean>(true)));
  }
}

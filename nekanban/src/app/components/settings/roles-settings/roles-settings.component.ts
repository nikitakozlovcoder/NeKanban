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
import { MatSnackBar } from "@angular/material/snack-bar";
import { TranslateService } from "@ngx-translate/core";
import {DialogService} from "../../../services/dialog.service";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";

@Component({
  selector: 'app-roles-settings',
  templateUrl: './roles-settings.component.html',
  styleUrls: ['./roles-settings.component.css']
})
export class RolesSettingsComponent implements OnInit, OnChanges {

  @Input() desk: Desk | undefined;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Output() rolesChange = new EventEmitter<Role[]>;
  @Input() desks: Desk[] = [];
  permissions: Permission[] = [];
  allPermissions: Permission[] = [];
  currentRole: Role | undefined;
  showAccordion = false;

  constructor(public dialog: MatDialog,
              private readonly rolesService: RolesService,
              public snackBar: MatSnackBar,
              public readonly translate: TranslateService,
              private readonly dialogService: DialogService,
              private readonly breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe(result => {
        this.showAccordion = result.matches;
      })
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
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.roles = result;
        this.rolesChange.emit(this.roles);
      }
    });
  }

  openRoleUpdatingDialog(role: Role, event: MouseEvent) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(RoleUpdatingComponent, {
      data: {role},
      width: '400px',
    });

    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.roles = result;
        this.rolesChange.emit(this.roles);
        this.updateCurrentRole();
      }
    });
  }

  openRoleDeletingDialog(role: Role, $event: MouseEvent) {
    $event.stopPropagation();
    const dialogRef = this.dialog.open(ConfirmationComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result == DialogActionTypes.Reject) {
        return;
      }

      this.rolesService.deleteRole(role.id).subscribe({
        next: (data: Role[]) => {
          this.roles = data;
          this.rolesChange.emit(this.roles);
          if (this.currentRole?.id == role.id){
            this.currentRole = this.roles.find(x => x.isDefault);
          }
        },error: (error: HttpErrorResponse) => {
          this.dialogService.openToast(error.error);
          /*if (error.error === "CantDeleteRoleWhenAnyUserHasThisRole") {
            this.snackBar.open('Невозможно удалить роль, на которую назначен хотя бы один пользователь!', undefined, {duration:2000, panelClass: ['big-sidenav']})
          }*/
        }
      });
    });
  }

  currentRoleHasPermission(permission: Permission) {
    return this.currentRole?.permissions.some(el => el.permission === permission.permission);
  }

  roleHasPermission(role: Role, permission: Permission) {
    return role.permissions.some(el => el.permission === permission.permission);
  }

  grantPermissionToRole(role: Role, permission: Permission) {
    this.rolesService.grantPermission(role.id, permission.permission).subscribe(result => {
      role.permissions.push(permission);
    })
  }

  revokePermissionFromRole(permission: Permission) {
    this.rolesService.revokePermission(this.currentRole!.id, permission.permission).subscribe(result => {
      this.currentRole!.permissions = this.currentRole!.permissions.filter(el => {
        return el.permission != permission.permission;
      });
    })
  }

  setRoleAsDefault(role: Role) {
    this.rolesService.setAsDefault(role.id).subscribe(result => {
      this.roles = result;
      this.rolesChange.emit(this.roles);
      this.updateCurrentRole();
    })
  }

  private updateCurrentRole() {
    this.currentRole = this.roles.find(el => el.id === this.currentRole!.id);
  }
}

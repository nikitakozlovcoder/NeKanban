import {Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import {Desk} from "../../../models/desk";
import {Role} from "../../../models/Role";
import {Permission} from "../../../models/permission";
import {MatListModule} from '@angular/material/list';
import {DeskCreationComponent} from "../../desk/desk-creation/desk-creation.component";
import {UntypedFormControl, Validators} from "@angular/forms";
import {MatDialog} from "@angular/material/dialog";
import {RoleCreationComponent} from "../dialogs/role-creation/role-creation.component";
import {RoleUpdatingComponent} from "../dialogs/role-updating/role-updating.component";
import {ConfirmationComponent} from "../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {RolesService} from "../../../services/roles.service";

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
  isActive = true;
  currentRole: Role | undefined;
  constructor(public dialog: MatDialog,
              private readonly rolesService: RolesService) { }

  ngOnInit(): void {
  }
  ngOnChanges() {
    this.currentRole = this.roles[0];
    this.allPermissions = this.rolesService.permissions;
  }

  getPermissionsForRole(role: Role) {
    this.currentRole = role;
  }

  openRoleCreationDialog() {
    const dialogRef = this.dialog.open(RoleCreationComponent, {
      data: {deskId: this.desk?.id},
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.roles = result;
        this.rolesChange.emit(this.roles);
      }
    });
  }

  openRoleUpdatingDialog(role: Role) {
    const dialogRef = this.dialog.open(RoleUpdatingComponent, {
      data: {role},
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.roles = result;
        this.rolesChange.emit(this.roles);
      }
    });
  }

  openRoleDeletingDialog(role: Role) {
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result == DialogActionTypes.Reject) {
        return;
      }
      this.rolesService.deleteRole(role.id).subscribe(result => {
        this.roles = result;
        this.rolesChange.emit(this.roles);
      });
    });
  }

  currentRoleHasPermission(permission: Permission) {
    return this.currentRole?.permissions.some(el => el.permission === permission.permission);
  }

  grantPermissionToRole(permission: Permission) {
    this.rolesService.grantPermission(this.currentRole!.id, permission.permission).subscribe(result => {
      this.currentRole?.permissions.push(permission);
    })
  }

  revokePermissionFromRole(permission: Permission) {
    this.rolesService.revokePermission(this.currentRole!.id, permission.permission).subscribe(result => {
      this.currentRole!.permissions = this.currentRole!.permissions.filter(el => {
        return el.permission != permission.permission;
      });
    })
  }

}

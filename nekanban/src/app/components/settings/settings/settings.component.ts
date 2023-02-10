import { Component, OnInit } from '@angular/core';
import {Desk} from "../../../models/desk";
import {UntypedFormControl, Validators} from "@angular/forms";
import {DeskService} from "../../../services/desk.service";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {ActivatedRoute, Router} from "@angular/router";
import {DeskUser} from "../../../models/deskUser";
import {DeskCreationComponent} from "../../desk/desk-creation/desk-creation.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  events: string[] = [];
  opened: boolean;
  desks: Desk[] = [];
  desk: Desk | undefined;
  isLoaded = true;
  areRolesLoaded = true;
  roles : Role[] = [];
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(6)]);

  constructor(private readonly deskService: DeskService,
              private rolesService: RolesService,
              private route: ActivatedRoute,
              private readonly router: Router,
              private readonly dialog: MatDialog) {
    this.opened = false;
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.loadDesks(parseInt(params['id']));
    })
  }

  changeDesk(id: number) {
    this.router.navigate(['/desks', id]);
  }

  loadCurrentDesk(deskId: number) {
    this.deskService.getDesk(deskId).subscribe(result => {
      this.desk = result;
      this.name.setValue(this.desk.name);
      this.initRolesForDesk();
    });
  }

  loadDesks(deskId: number) {
    this.isLoaded = false;
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {
        this.desks = data;
        if (!this.desks.some(el => el.id === deskId)) {
          this.router.navigate(['/**'], { skipLocationChange: true });
          return;
        }
      },
      error: () => {
      },
      complete: () => this.loadCurrentDesk(deskId)
    });
  }
  private initRolesForDesk() {
    this.rolesService.getRoles(this.desk!.id).subscribe(result => {
      this.roles = result;
      this.isLoaded = true;
    });
  }

  getCurrentUser() {
    return JSON.parse(localStorage.getItem("currentUser")!);
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.desk?.id);
  }

  getDeskOwner() {
    return this.desk?.deskUsers.find(el => el.isOwner);
  }

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  showDeskCreation() {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.router.navigate(['/desks', result.id]);
      }
    });
    this.opened = false;
  }

  hasAnyPermissionsForGeneralSettings() {
    return this.checkUserPermission(this.getCurrentDesk()!.deskUser, "UpdateGeneralDesk") ||
      this.checkUserPermission(this.getCurrentDesk()!.deskUser, "ViewInviteLink") ||
      this.checkUserPermission(this.getCurrentDesk()!.deskUser, "ManageInviteLink") ||
      this.checkUserPermission(this.getCurrentDesk()!.deskUser, "DeleteDesk");
  }
}

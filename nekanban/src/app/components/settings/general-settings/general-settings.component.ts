import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskUser} from "../../../models/deskUser";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {DeskUserService} from "../../../services/deskUser.service";
import {UntypedFormControl, Validators} from "@angular/forms";
import {DeskService} from "../../../services/desk.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.css']
})
export class GeneralSettingsComponent implements OnInit {

  @Input() desk: Desk | undefined;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Input() desks: Desk[] = [];
  @Output() desksChange = new EventEmitter<Desk[]>;
  isRemoveDeskLoaded = true;
  isNameUpdateLoaded = true;
  isLinkLoaded = true;
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(6)]);
  constructor(private readonly rolesService: RolesService,
              private readonly deskUserService: DeskUserService,
              private readonly deskService: DeskService,
              public snackBar: MatSnackBar,
              private router: Router) {
  }

  ngOnInit(): void {
    this.name = new UntypedFormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
  }

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.desk!.id);
  }

  updateDesk() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isNameUpdateLoaded = false;
      this.deskService.updateDesk(this.desk!.id, this.name.value).subscribe({
        next: (data: Desk) => {
          this.isNameUpdateLoaded = true;
          this.desk = data;
          this.deskChange.emit(this.desk);
          let index = this.desks.findIndex(el => el.id === this.desk!.id);
          this.desks[index].name = data.name;
          this.desksChange.emit(this.desks);
        }
      })
    }
  }

  hasInviteLink() {
    return !(this.desk?.inviteLink === null);
  }

  getInviteLink() {
    if (this.hasInviteLink()) {
      return environment.angularBaseUrl + "invite?desk=" + this.desk?.inviteLink;
    }
    return null;
  }

  generateLink() {
    this.isLinkLoaded = false;
    this.deskService.setLink(this.desk!.id).subscribe( {
      next: data => {
        this.isLinkLoaded = true;
        this.desk = data;
      },
      error: () => {
      }
    })
  }

  removeDesk(deskId: number) {
    this.isRemoveDeskLoaded = false;
    this.deskService.removeDesk(deskId).subscribe({
      next: () => {
        this.isRemoveDeskLoaded = true;
        this.router.navigate(['']).then();
      },
      error: () => {
      }
    })
  }
}
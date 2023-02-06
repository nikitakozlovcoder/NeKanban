import { Component, OnInit } from '@angular/core';
import {Desk} from "../../../models/desk";
import {UntypedFormControl, Validators} from "@angular/forms";
import {DeskService} from "../../../services/desk.service";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {ActivatedRoute} from "@angular/router";
import {DeskUser} from "../../../models/deskUser";

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
  currentId: number = -1;
  roles : Role[] = [];
  private sub: any;
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(6)]);

  constructor(private readonly deskService: DeskService,
              private rolesService: RolesService,
              private route: ActivatedRoute) {
    this.opened = false;
  }

  ngOnInit(): void {
    this.loadDesks();
    this.sub = this.route.params.subscribe(params => {
      this.deskService.getDesk(params['id']).subscribe(result => {
        this.desk = result;
        this.name.setValue(this.desk.name);
        this.initRolesForDesk();
      });
    })
  }

  changeDesk(id: number) {
    this.currentId = id;
    this.opened = false;
    this.isLoaded = false;
    this.deskService.getDesk(id).subscribe({
      next: (data: Desk) => {
        this.desk = data;
      },
      error: () => {
      }
    })
  }

  loadDesks() {
    this.isLoaded = false;
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {

        this.desks = data;
        this.isLoaded = true;
      },
      error: () => {
      }
    });
  }
  private initRolesForDesk() {
    this.rolesService.getRoles(this.desk!.id).subscribe(result => {
      this.roles = result;
      this.areRolesLoaded = true;
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
}

import { Component, OnInit } from '@angular/core';
import {Desk} from "../../../models/desk";
import {UntypedFormControl, Validators} from "@angular/forms";
import {DeskService} from "../../../services/desk.service";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {ActivatedRoute, Router} from "@angular/router";
import {DeskUserService} from "../../../services/deskUser.service";
import {BehaviorSubject, map, combineLatest, combineLatestWith, filter} from "rxjs";

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
  desksLoaded = new BehaviorSubject(false);
  currentDeskLoaded = new BehaviorSubject(false);
  rolesLoaded = new BehaviorSubject(false);
  get isLoaded() {
    return combineLatest([this.desksLoaded, this.currentDeskLoaded, this.rolesLoaded]).
    pipe(map(x => x.every(isLoaded => isLoaded)));
  }
  roles : Role[] = [];
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(6)]);

  constructor(private readonly deskService: DeskService,
              public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly route: ActivatedRoute,
              private readonly router: Router) {
    this.opened = false;
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.loadDesks(parseInt(params['id']));
      this.loadCurrentDesk(parseInt(params['id']));
      this.initRolesForDesk(parseInt(params['id']));
    })
    this.desksLoaded.pipe(combineLatestWith(this.currentDeskLoaded)).pipe(
      combineLatestWith(this.rolesLoaded), filter(el => el[0].every(x => x === true) && el[1] === true)
      ).subscribe(result => {
        this.redirectIfDontHaveAccessToSettings();
    })
  }

  loadCurrentDesk(deskId: number) {
    this.deskService.getDesk(deskId).subscribe({
      next: (result) => {
        this.desk = result;
        this.currentDeskLoaded.next(true);
        this.name.setValue(this.desk.name);
      }
    });
  }

  loadDesks(deskId: number) {
    this.desksLoaded.next(false);
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {
        this.desks = data;
        this.desksLoaded.next(true);
        if (!this.desks.some(el => el.id === deskId)) {
          this.router.navigate(['/**'], { skipLocationChange: true }).then();
          return;
        }
      },
      error: () => {
      }
    });
  }

  redirectIfDontHaveAccessToSettings() {
    if (!this.rolesService.userHasAtLeastOnePermissionForAllSettings(this.roles, this.deskUserService.getCurrentDeskUser(this.desk)!)) {
      this.router.navigate(['/**']).then();
    }
  }

  private initRolesForDesk(deskId: number) {
    this.rolesService.getRoles(deskId).subscribe({
      next: (result) => {
        this.roles = result;
        this.rolesLoaded.next(true);
      }
    });
  }

  getCurrentUser() {
    return JSON.parse(localStorage.getItem("currentUser")!);
  }
}

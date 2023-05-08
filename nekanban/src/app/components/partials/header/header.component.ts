import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {User} from "../../../models/user";
import {Desk} from "../../../models/desk";
import {UserService} from "../../../services/user.service";
import {Router} from "@angular/router";
import {MatSidenav} from "@angular/material/sidenav";
import {BehaviorSubject, Subscription} from "rxjs";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {DeskUserService} from "../../../services/deskUser.service";
import {UntilDestroy} from "@ngneat/until-destroy";

@UntilDestroy({checkProperties: true})
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  @Input() currentUser: User | undefined;
  desk: Desk | undefined;
  @Input() currentDeskId: number | undefined;
  //@Output() deskChange = new EventEmitter<Desk>;
  isFavouriteLoaded = new BehaviorSubject(true);
  @Input() sidenav : MatSidenav | undefined;
  @Input() desks: Desk[] = [];
  @Output() desksChange = new EventEmitter<Desk[]>;
  @Input() opened = false;
  @Output() openedChange = new EventEmitter<boolean>();
  isLogoutLoaded = new BehaviorSubject(true);
  isMobile = false;
  private subscriptions = new Subscription();

  constructor(private readonly userService: UserService,
              private router: Router,
              private readonly deskUserService: DeskUserService,
              private readonly breakpointObserver: BreakpointObserver) { }


  ngOnInit(): void {
    this.subscriptions.add(this.breakpointObserver.observe(Breakpoints.HandsetPortrait).subscribe(result => {
      this.isMobile = result.matches;
    }));
    this.setCurrentDesk();
  }

  logout() {
    this.isLogoutLoaded.next(false);
    this.userService.logoutUser().subscribe({
      next: () => {
        this.isLogoutLoaded.next(true);
        this.router.navigate(['authorization']).then();
      }
    });
  }

  addToFavourite(index: number |undefined) {
    let founded = this.desks.find(el => el.deskUser.preference === 1);
    this.isFavouriteLoaded.next(false);
    if (founded != undefined) {
      this.deskUserService.addPreference(founded.id, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.setCurrentDesk();
          this.desksChange.emit(this.desks);
        }
      }).add(() => {
        this.isFavouriteLoaded.next(true);
      });
    }

    if (index != undefined) {
      this.deskUserService.addPreference(index, 1).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.desksChange.emit(this.desks);
          this.setCurrentDesk();
        }
      }).add(() => {
        this.isFavouriteLoaded.next(true);
      });
    }
  }

  removeFromFavourites(index: number |undefined) {
    if (index != undefined) {
      this.isFavouriteLoaded.next(false);
      this.deskUserService.addPreference(index, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data
          this.desksChange.emit(this.desks);
          this.setCurrentDesk();
        }
      }).add(() => {
        this.isFavouriteLoaded.next(true);
      });
    }
  }

  setCurrentDesk() {
    this.desk = this.desks.find(el => el.id === this.currentDeskId);
  }

  toggleSidenav() {
    this.opened = !this.opened;
    this.openedChange.emit(this.opened);
  }
}

import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {User} from "../../../models/user";
import {Desk} from "../../../models/desk";
import {UserService} from "../../../services/user.service";
import {Router} from "@angular/router";
import {MatSidenav} from "@angular/material/sidenav";
import {DeskService} from "../../../services/desk.service";

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
  isFavouriteLoaded = true;

  @Input() sidenav : MatSidenav | undefined;

  @Input() desks: Desk[] = [];
  @Output() desksChange = new EventEmitter<Desk[]>;

  @Input() opened = false;
  @Output() openedChange = new EventEmitter<boolean>();

  constructor(private readonly userService: UserService,
              private router: Router,
              private readonly deskService: DeskService) { }


  ngOnInit(): void {
    this.setCurrentDesk();
  }

  logout() {
    this.userService.logoutUser();
    this.router.navigate(['authorization']);
  }

  addToFavourite(index: number |undefined) {
    let founded = this.desks.find(el => el.deskUser.preference === 1);
    this.isFavouriteLoaded = false;
    if (founded != undefined) {
      this.deskService.addPreference(founded.id, 0).subscribe({
        next: (data: Desk[]) => {
          this.isFavouriteLoaded = true;
          this.desks = data;
          this.setCurrentDesk();
          this.desksChange.emit(this.desks);
        }
      });
    }

    if (index != undefined) {
      this.deskService.addPreference(index, 1).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.isFavouriteLoaded = true;
          this.desksChange.emit(this.desks);
          this.setCurrentDesk();
        }
      });
    }
  }

  removeFromFavourites(index: number |undefined) {
    if (index != undefined) {
      this.isFavouriteLoaded = false;
      this.deskService.addPreference(index, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.isFavouriteLoaded = true;
          this.desksChange.emit(this.desks);
          this.setCurrentDesk();
        }
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

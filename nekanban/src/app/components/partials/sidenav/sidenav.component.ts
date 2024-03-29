import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskCreationComponent} from "../../desk/desk-creation/desk-creation.component";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {MatSidenav} from "@angular/material/sidenav";
import {DeskUserService} from "../../../services/deskUser.service";
import {ConfirmationComponent} from "../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {filter, switchMap} from "rxjs";

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent implements OnInit {

  @ViewChild('my-sidenav') sidenav: MatSidenav | undefined;
  events: string[] = [];

  @Input() opened: boolean;
  @Output() openedChange = new EventEmitter<boolean>();
  @Input() desks: Desk[] = [];
  @Output() exitEvent = new EventEmitter<number>;
  @Input() currentDeskId?: number;

  constructor(private readonly dialog: MatDialog,
              private readonly router: Router,
              public readonly deskUserService: DeskUserService) {
    this.opened = false;
  }

  ngOnInit(): void {
  }

  closeSidenav() {
    this.opened = false;
    this.openedChange.emit(this.opened);
  }

  showDeskCreation() {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().pipe(filter(x => x)).subscribe( result => {
      this.router.navigate(['/desks', result.id]).then();
    });
    this.opened = false;
    this.openedChange.emit(this.opened);
  }

  exitFromDesk(desk: Desk) {
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().pipe(filter(x => x === DialogActionTypes.Accept),
      switchMap(() => this.deskUserService.exitFromDesk(desk.id))).subscribe(() => {
        if (desk.id === this.currentDeskId) {
          this.router.navigate(['']).then();
        }
        else {
          this.closeSidenav();
          this.exitEvent.emit(desk.id);
        }
    });
  }
}

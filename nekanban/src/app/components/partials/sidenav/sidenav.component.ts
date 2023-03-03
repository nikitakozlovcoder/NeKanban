import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskCreationComponent} from "../../desk/desk-creation/desk-creation.component";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {MatSidenav} from "@angular/material/sidenav";

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

  constructor(private readonly dialog: MatDialog,
              private readonly router: Router) {
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
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.router.navigate(['/desks', result.id]);
      }
    });
    this.opened = false;
    this.openedChange.emit(this.opened);
  }
}

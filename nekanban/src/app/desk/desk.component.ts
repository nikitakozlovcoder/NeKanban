import { Component, OnInit } from '@angular/core';
import {Desk} from "../models/desk";
import {DeskService} from "../services/desk.service";
import {UserService} from "../services/user.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DialogComponent} from "../dialog/dialog.component";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";

@Component({
  selector: 'app-desk',
  templateUrl: './desk.component.html',
  styleUrls: ['./desk.component.css']
})
export class DeskComponent implements OnInit {

  events: string[] = [];
  opened: boolean;
  desks: Desk[] = [];
  index: number = 0;
  //desk: Desk;
  constructor(private deskService: DeskService, private userService: UserService, private router: Router, public dialog: MatDialog) {
    this.opened = false;

    //this.desk = this.desks[0];
  }

  ngOnInit(): void {
    this.desks = this.deskService.getDesks();

  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '250px',
    });

  }
  changeDesk(index: number) {
    this.index = index;
  }
  getDesk() : Desk {
    return this.desks[this.index];
  }
  logout() {
    this.userService.logoutUser();
    this.router.navigate(['authorization']);
  }
  showDeskCreation() {
    this.openDialog();
  }

}

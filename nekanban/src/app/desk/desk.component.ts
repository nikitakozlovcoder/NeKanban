import { Component, OnInit } from '@angular/core';
import {Desk} from "../models/desk";
import {DeskService} from "../services/desk.service";
import {UserService} from "../services/user.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DialogComponent} from "../dialog/dialog.component";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";
import {FormControl, Validators} from "@angular/forms";

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
  changed_index: number = -1;
  //desk: Desk;
  constructor(private deskService: DeskService, private userService: UserService, private router: Router, public dialog: MatDialog) {
    this.opened = false;

    //this.desk = this.desks[0];
  }

  ngOnInit(): void {
    this.desks = this.deskService.getDesks();
    console.log(this.desks);


  }
  test: string[] = ['hi', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'k', 'i'];
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);
  panelOpenState = false;
  openDialog(): void {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '250px',
    });

  }
  closeDialog() {
    this.dialog.closeAll();
  }
  changeDesk(index: number) {
    this.changed_index = index;
    this.opened = false;
  }
  getDesk() : Desk {
   //console.log(this.desks.length);
    if (this.changed_index === -1) {
      this.desks.forEach( (el, index) => {
        if (el.deskUser.preference === 1) {
          this.index = index;
        }
      })
      return this.desks[this.index];
    }
    return this.desks[this.changed_index]
  }
  logout() {
    this.userService.logoutUser();
    this.router.navigate(['authorization']);
  }
  showDeskCreation() {
    this.openDialog();
  }
  addToFavourite(index: number) {
    this.deskService.addPreference(index, 1).subscribe({
      next: (data: Desk[]) => {
        this.desks = data;
      }
    });
  }
  removeFromFavourites(index: number) {
    this.deskService.addPreference(index, 0);
  }
  updateDesk(index: number) {
    this.deskService.updateDesk(index, this.name.value);
    //this.refresh();
  }
  refresh(): void {
    window.location.reload();
  }


}

import { Component, OnInit } from '@angular/core';
import {Desk} from "../models/desk";
import {DeskService} from "../services/desk.service";
import {UserService} from "../services/user.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DialogComponent} from "../dialog/dialog.component";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";
import {FormControl, Validators} from "@angular/forms";
import {Observable} from "rxjs";
import {CdkDragDrop, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import {TaskCreationComponent} from "../task-creation/task-creation.component";

@Component({
  selector: 'app-desk',
  templateUrl: './desk.component.html',
  styleUrls: ['./desk.component.css']
})
export class DeskComponent implements OnInit {

  events: string[] = [];
  opened: boolean;
  desks: Desk[] = [];
  desk: Desk | undefined;
  index: number = 0;
  changed_index: number = -1;
  columns: string[] = ['To do', 'done'];
  //desk: Desk;
  todo = ['Get to work', 'Pick up groceries', 'Go home', 'Fall asleep'];

  done = ['Get up', 'Brush teeth', 'Take a shower', 'Check e-mail', 'Walk dog'];

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  constructor(private deskService: DeskService, private userService: UserService, private router: Router, public dialog: MatDialog) {
    this.opened = false;

    //console.log(this.desks);
    //this.desk = this.desks[0];
  }

  ngOnInit(): void {
    this.loadDesks();
  }
  test: string[] = ['hi', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'k', 'i'];
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);
  panelOpenState = false;
  loadDesks() {
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {
        this.desks = data;
        console.log(this.desks);
        let founded = this.desks.find(el => el.deskUser.preference === 1);
        if (founded != undefined) {
          let id = founded.id;
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;
              console.log("Preference founded");
              console.log(this.desk);
            },
            error: err => {
              console.log(err);

            }
          })
        }
        else {
          let id = this.desks[0].id;
          console.log("Preference not founded");
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;
              console.log(this.desk);
            },
            error: err => {
              console.log(err);

            }
          })
        }
      },
      error: err => {
        console.log(err);

      }
    });
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '250px',
    });
    dialogRef.afterClosed().subscribe( result => {
      this.desk = result;
      this.deskService.getDesks().subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
        },
        error: err => {
          console.log(err);
        }
      });
    });

  }
  openTaskCreationDialog(): void {
    const dialogRef = this.dialog.open(TaskCreationComponent, {
      //width: '500px',
    });

  }
  closeDialog() {
    this.dialog.closeAll();
  }
  changeDesk(id: number) {
    //.changed_index = index;
    this.opened = false;
    this.deskService.getDesk(id).subscribe({
      next: (data: Desk) => {
        this.desk = data;
        console.log("Changed desk to:");
        console.log(this.desk);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  getDesk() : Desk {
   //console.log(this.desks.length);
    if (this.changed_index === -1) {
      this.desks.forEach( (el, index) => {
        if (el.deskUser.preference === 1) {
          this.index = index;
        }
      })
      //console.log("HHHHHHH")
      //console.log(this.desks);
      return this.desks[this.index];
    }
    //console.log("UUUUUUU")
    //console.log(this.desks);
    return this.desks[this.changed_index];
  }
  logout() {
    this.userService.logoutUser();
    this.router.navigate(['authorization']);
  }
  showDeskCreation() {
    this.openDialog();
    this.opened = false;
    /*this.deskService.getDesk(this.desks[this.desks.length - 1].id).subscribe({
      next : (data: Desk) => {
        console.log(data);
        this.desk = data;
      },
      error: err => {
        console.log(err);

      }
    });*/
  }
  showTaskCreation() {
    this.openTaskCreationDialog();
  }
  addToFavourite(index: number |undefined) {
    /*this.desks.forEach( (el, index) => {
      this.deskService.addPreference(el.id, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
        }
      });
    })*/
    let founded = this.desks.find(el => el.deskUser.preference === 1);
    if (founded != undefined) {
      this.deskService.addPreference(founded.id, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
        }
      });
    }
    if (index != undefined) {
      this.deskService.addPreference(index, 1).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.deskService.getDesk(index).subscribe({
            next : (data: Desk) => {
              console.log("Current desk from favourites");
              console.log(data);
              this.desk = data;
            },
            error: err => {
              console.log(err);

            }
          });
        }
      });
    }

  }
  removeFromFavourites(index: number |undefined) {
    if (index != undefined) {
      this.deskService.addPreference(index, 0).subscribe({
        next: (data: Desk[]) => {
          this.desks = data;
          this.deskService.getDesk(index).subscribe({
            next : (data: Desk) => {
              //console.log(data);
              this.desk = data;
            },
            error: err => {
              console.log(err);

            }
          });
        }
      });
    }

  }
  updateDesk(id: number) {
    this.deskService.updateDesk(this.desk!.id, this.name.value).subscribe({
      next: (data: Desk) => {
        //console.log(data);
        /*let finded = this.desks.find( el => el.id === id);
        //console.log(finded);
        let index = -1;
        if (finded != undefined) {
          index = this.desks.indexOf(finded);
          this.desks[index] = data;
          console.log(this.desks);
        }*/
        this.desk = data;
      }
    })
    //this.refresh();
  }
  isCurrentDesk(index: number) {
    return this.changed_index === index || this.index === index;
  }

  getFavourite() {
    this.desks.find(el => el.deskUser.preference === 1);
  }
  getDeskOwner() {
    return this.desk?.deskUsers.find(el => el.role === 2);
  }

}

import { Component, OnInit } from '@angular/core';
import {Desk} from "../models/desk";
import {DeskService} from "../services/desk.service";
import {UserService} from "../services/user.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";
import {FormControl, Validators} from "@angular/forms";
import {CdkDragDrop, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import {TaskCreationComponent} from "../task-creation/task-creation.component";
import {Column} from "../models/column";
import {ColumnCreationComponent} from "../column-creation/column-creation.component";
import {ColumnService} from "../services/column.service";
import {Todo} from "../models/todo";
import {TodoService} from "../services/todo.service";
import {TodoCreationComponent} from "../todo-creation/todo-creation.component";
import {TodoEditingComponent} from "../todo-editing/todo-editing.component";
import {ColumnUpdatingComponent} from "../column-updating/column-updating.component";
import {MatSelectChange} from "@angular/material/select";
import {RolesService} from "../services/roles.service";
import {DeskUsers} from "../models/deskusers";
import {DeskRole} from "../models/deskrole";
import {Role} from "../models/Role";
import {DeskUserService} from "../services/deskUser.service";
import {BaseHttpService} from "../services/base_http.service";

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
  current_id: number = -1;
  columns: Column[] = [];
  toDos: Todo[] = [];
  clientBaseHref = "";
  //desk: Desk;

  currentRoles : DeskRole[] = [];
  roles : Role[] = [new Role(0, "Участник"), new Role(1, "Менеджер")];
  roleNames: string[] = ["Участник", "Менеджер", "Руководитель"];
  //rolesControl = new FormControl();

  drop(event: CdkDragDrop<Todo[]>, columnId: number) {
    if (event.previousContainer === event.container) {
      let position;
      if (event.previousIndex < event.currentIndex) {
        position = event.container.data[event.currentIndex].order + 1;
      }
      else {
        position = event.container.data[event.currentIndex].order;
      }
      this.todoService.moveToDo(event.container.data[event.previousIndex].id, event.container.data[event.previousIndex].column.id, position).subscribe({
        next: data => {
          this.toDos = data;
        },
        error: err => {
          console.log(err);
        }
      })
      console.log(event.container.data, event.previousIndex, event.currentIndex);
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      console.log(columnId);
      console.log(event.previousContainer);
      console.log(event.previousIndex);
      console.log(event.container);
      console.log(event.currentIndex);
      /*let previousContainer = event.previousContainer;
      let previousIndex = event.previousIndex;
      let container = event.container;
      let currentIndex = event.currentIndex;*/
      /*transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );*/
      console.log("HIIIIIIIII");
      console.log(event.previousContainer);
      console.log(event.previousIndex);
      console.log(event.container);
      console.log(event.currentIndex);
      let newOrder;
      let newIndex;
      if (event.container.data.length > 0) {
        if (event.currentIndex == event.container.data.length) {
          console.log("yes");
          newOrder = event.container.data[event.currentIndex-1].order + 1;
        }
        else {
          console.log("no");
          newOrder = event.container.data[event.currentIndex].order;
        }
        //newIndex = event.currentIndex;
      }
      else {
        newOrder = 0;
      }
      /*console.log(previousContainer.data);
      console.log(previousContainer);*/
      this.todoService.moveToDo(event.previousContainer.data[event.previousIndex].id, columnId, newOrder).subscribe({
        next: data => {
          this.toDos = data;
        },
        error: err => {
          console.log(err);
        }
      });
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  drop_column(event: CdkDragDrop<Column[]>) {
    console.log("Container data");
    console.log(event.container.data);
    console.log("Previous index");
    console.log(event.previousIndex);
    console.log("Current index");
    console.log(event.currentIndex);
    if (event.previousIndex != 0 && event.previousIndex != event.container.data.length - 1 && event.currentIndex != 0 && event.currentIndex != event.container.data.length - 1) {
      let position;
      if (event.previousIndex < event.currentIndex) {
        position = event.container.data[event.currentIndex].order + 1;
      }
      else {
        position = event.container.data[event.currentIndex].order;
      }
      this.columnService.moveColumn(event.container.data[event.previousIndex].id, position).subscribe({
        next: data => {
          this.columns = data.sort(function (a: Column, b: Column) {
            if (a.order > b.order) {
              return 1;
            }
            if (a.order < b.order) {
              return -1;
            }
            return 0;
          });
          this.reloadTodosForColumns(this.toDos);
        },
        error: err => {
          console.log(err);
        }
      })
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }


    /*if (event.previousContainer === event.container) {
      console.log("same container");
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      console.log("different container");
      this.columnService.moveColumn(event.container.data[event.currentIndex].id, event.currentIndex).subscribe({
        next: data => {
          this.columns = data;
        },
        error: err => {
          console.log(err);
        }
      })
      /*transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }*/
  }

  constructor(private deskService: DeskService, private userService: UserService, private router: Router, public dialog: MatDialog, private columnService: ColumnService,
              private todoService: TodoService, private rolesService: RolesService, private deskUserService: DeskUserService, private http_service: BaseHttpService) {
    this.opened = false;

    //console.log(this.desks);
    //this.desk = this.desks[0];
  }

  ngOnInit(): void {
    this.loadDesks();
    this.rolesService.initRoles();
    this.clientBaseHref = window.location.href;
    //console.log(JSON.parse(localStorage.getItem("currentUser")!));
  }
  name = new FormControl('', [Validators.required, Validators.minLength(6)]);
  panelOpenState = false;
  loadDesks() {
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {
        this.desks = data;
        console.log(this.desks);
        let founded = this.desks.find(el => el.deskUser.preference === 1);

        if (founded != undefined) {
          let id = founded.id;
          this.current_id = id;
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;
              console.log("Preference founded");
              console.log(this.desk);
              this.getColumns();
              this.getToDos(this.desk.id);
            },
            error: err => {
              console.log(err);

            }
          })
        }
        else {
          let id = this.desks[0].id;
          this.current_id = id;
          console.log("Preference not founded");
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;
              console.log(this.desk);
              this.getColumns();
              this.getToDos(this.desk.id);
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
      width: '400px',
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        console.log("Not undefined");
        console.log(result);
        this.desk = result;
        this.current_id = this.desk!.id;
        this.deskService.getDesks().subscribe({
          next: (data: Desk[]) => {
            this.desks = data;
            console.log(data);
            this.getColumns();
          },
          error: err => {
            console.log(err);
          }
        });
      }
      else {
        console.log("Undefined");
      }
    });

  }
  openTaskCreationDialog(todo: Todo): void {
    console.log(this.toDos);
    const dialogRefView = this.dialog.open(TaskCreationComponent, {
      data: {todo: todo, desk: this.desk, deskUser: this.getCurrentDesk()!.deskUser}
      //width: '500px',
    });
  dialogRefView.afterClosed().subscribe( result => {
    /*console.log(result);
    console.log(this.toDos);
    console.log(this.columns);*/
    let idx = this.toDos.indexOf(todo);
    this.toDos[idx] = result;
    console.log(this.toDos);
    this.reloadTodosForColumns(this.toDos);
    console.log(this.columns);
  })
  }
  reloadTodosForColumns(todos : Todo[]) {
    for (let i = 0; i < this.columns.length; i++) {
      this.columns[i].todos = todos.filter(todo => todo.column.id === this.columns[i].id).sort(function (a: Todo, b: Todo) {
        if (a.order > b.order) {
          return 1;
        }
        if (a.order < b.order) {
          return -1;
        }
        return 0;
      });
    }
  }
  openColumnCreationDialog() {
    const dialogRef = this.dialog.open(ColumnCreationComponent, {
      data: {deskId: this.desk?.id},
      width: '400px'
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.columns = result.sort(function (a: Column, b: Column) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.reloadTodosForColumns(this.toDos);

        /*this.deskService.getDesks().subscribe({
          next: (data: Desk[]) => {
            this.desks = data;
          },
          error: err => {
            console.log(err);
          }
        });*/
      }
    })
  }
  closeDialog() {
    this.dialog.closeAll();
  }
  changeDesk(id: number) {
    this.current_id = id;
    this.opened = false;
    this.deskService.getDesk(id).subscribe({
      next: (data: Desk) => {
        this.desk = data;
        console.log("Changed desk to:");
        console.log(this.desk);
        this.getColumns();
        this.getToDos(this.desk.id);
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
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.deskService.updateDesk(this.desk!.id, this.name.value).subscribe({
        next: (data: Desk) => {
          this.desk = data;
          let index = this.desks.findIndex(el => el.id === this.desk!.id);
          this.desks[index].name = data.name;
        }
      })
    }
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
  getCurrentDesk() {
    return this.desks.find(el => el.id === this.current_id);
  }
  getColumns() {
    console.log("hi");
    this.columnService.getColumns(this.desk!.id).subscribe({
      next: data => {
        this.columns = data.sort(function (a, b) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.reloadTodosForColumns(this.toDos);
        //this.columns
        console.log(this.columns);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  removeColumn(columnId: number) {
    this.columnService.removeColumn(columnId).subscribe({
      next: data => {
        this.columns = data.sort(function (a: Column, b: Column) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.reloadTodosForColumns(this.toDos);

      },
      error: err => {
        console.log(err);

      }
    })
  }

  generateLink() {
    this.deskService.setLink(this.desk!.id).subscribe( {
      next: data => {
        this.desk = data;
        console.log(this.desk);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  hasInviteLink() {
    return !(this.desk?.inviteLink === null);
  }
  hasAccessToInviteLink() {
    return this.getDeskOwner()!.user.id === this.getCurrentUser().id;
  }
  getInviteLink() {
    if (this.hasInviteLink()) {
      return this.clientBaseHref + "invite?desk=" + this.desk?.inviteLink;
    }
    return null;
  }
  getCurrentUser() {
    return JSON.parse(localStorage.getItem("currentUser")!);
  }

  removeUser(usersId: number[]) {
    this.deskService.removeUserFromDesk(usersId, this.desk!.id).subscribe({
      next: data => {
        this.desk = data;
        //console.log(this.desk);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  removeDesk(deskId: number) {
    this.deskService.removeDesk(deskId).subscribe({
      next: data => {
        this.loadDesks();
        //console.log(this.desk);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  getToDos(deskId: number) {
    this.todoService.getToDos(deskId).subscribe({
      next: data => {
        this.toDos = data;
        for (let i = 0; i < this.columns.length; i++) {
          this.columns[i].todos =  data.filter( todo => todo.column.id === this.columns[i].id).sort(function (a: Todo, b: Todo) {
            if (a.order > b.order) {
              return 1;
            }
            if (a.order < b.order) {
              return -1;
            }
            return 0;
          });
        }
        console.log(this.toDos);
        //console.log(this.desk);
      },
      error: err => {
        console.log(err);

      }
    })
  }
  getToDosForColumn(columnId: number) {
    return this.toDos.filter( todo => todo.column.id === columnId).sort(function (a: Todo, b: Todo) {
      if (a.order > b.order) {
        return 1;
      }
      if (a.order < b.order) {
        return -1;
      }
      return 0;
    });
  }
  openToDoCreationDialog() {
    const dialogRef = this.dialog.open(TodoCreationComponent, {
      data: {deskId: this.desk?.id, isEdit: false},
      width: '400px'
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.toDos = result;
        for (let i = 0; i < this.columns.length; i++) {
          this.columns[i].todos =  result.filter( (todo : Todo) => todo.column.id === this.columns[i].id).sort(function (a: Todo, b: Todo) {
            if (a.order > b.order) {
              return 1;
            }
            if (a.order < b.order) {
              return -1;
            }
            return 0;
          });
        }
        /*this.deskService.getDesks().subscribe({
          next: (data: Desk[]) => {
            this.desks = data;
          },
          error: err => {
            console.log(err);
          }
        });*/
      }
    })
  }
  openToDoEditingDialog(todo: Todo) {
    const dialogRef = this.dialog.open(TodoEditingComponent, {
      data: {deskId: this.desk?.id, toDo: todo},
      width: '400px'
    });
    dialogRef.afterClosed().subscribe( result => {
      //debugger
      if (result !== undefined) {
        console.log("HIII22222");
        this.toDos[this.toDos.indexOf(todo)] = result;
        this.reloadTodosForColumns(this.toDos);
      }
      else {
        console.log("HIII");
      }
      console.log(this.toDos);
    })
  }

  openColumnUpdatingDialog(column: Column) {
    const dialogRef = this.dialog.open(ColumnUpdatingComponent, {
      data: {deskId: this.desk?.id, column: column},
      width: '400px'
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.columns = result.sort(function (a: Column, b: Column) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.reloadTodosForColumns(this.toDos);
        /*this.deskService.getDesks().subscribe({
          next: (data: Desk[]) => {
            this.desks = data;
          },
          error: err => {
            console.log(err);
          }
        });*/
      }
    })
  }

  changeUserRole(event: MatSelectChange, deskUserId: number) {
    this.deskUserService.changeRole(deskUserId, event.value).subscribe({
      next: (data: DeskUsers[]) => {
        this.desk!.deskUsers = data;
      },
      error: err => {
        console.log(err);
      }
    });
  }
  checkUserPermission(deskUser: DeskUsers, permissionName: string) {
    return this.rolesService.userHasPermission(deskUser, permissionName);
  }

}

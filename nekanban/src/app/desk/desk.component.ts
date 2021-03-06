import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
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
  isLoaded = false;
  isNameUpdateLoaded = true;
  isColumnDeleteLoaded: boolean[] = [];
  isLinkLoaded = true;
  isRemoveDeskLoaded = true;
  isUserRemoveLoaded = true;
  colorWarn = "warn";
  isFavouriteLoaded = true;
  //desk: Desk;

  currentRoles : DeskRole[] = [];
  roles : Role[] = [new Role(0, "????????????????"), new Role(1, "????????????????")];
  roleNames: string[] = ["????????????????", "????????????????", "????????????????????????"];
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
          this.reloadTodosForColumns(this.toDos);
        },
        error: _ => {
        }
      })
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      let newOrder;
      if (event.container.data.length > 0) {
        if (event.currentIndex == event.container.data.length) {
          newOrder = event.container.data[event.currentIndex-1].order + 1;
        }
        else {
          newOrder = event.container.data[event.currentIndex].order;
        }
      }
      else {
        newOrder = 0;
      }

      this.todoService.moveToDo(event.previousContainer.data[event.previousIndex].id, columnId, newOrder).subscribe({
        next: data => {
          this.toDos = data;
          this.reloadTodosForColumns(this.toDos);
        },
        error: _ => {
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
        error: _ => {
        }
      })
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
  }

  constructor(private deskService: DeskService, private userService: UserService, private router: Router, public dialog: MatDialog, private columnService: ColumnService,
              private todoService: TodoService, private rolesService: RolesService, private deskUserService: DeskUserService) {
    this.opened = false;
  }

  ngOnInit(): void {
    this.loadDesks();
    this.rolesService.initRoles();
    this.clientBaseHref = window.location.href;
  }
  /*ngAfterViewInit() {
    this.cdr.detectChanges();
  }*/
  name = new FormControl('', [Validators.required, Validators.minLength(6)]);
  panelOpenState = false;
  loadDesks() {
    this.isLoaded = false;
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {

        this.desks = data;
        if (this.desks.length === 0){
          this.isLoaded = true;
          return;
        }
        let founded = this.desks.find(el => el.deskUser.preference === 1);

        if (founded != undefined) {
          let id = founded.id;
          this.current_id = id;
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;

              this.getColumns();
              this.getToDos(this.desk.id);
              this.name = new FormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
            },
            error: () => {
            }
          })
        }
        else {
          let id = this.desks[0].id;
          this.current_id = id;
          this.deskService.getDesk(id).subscribe({
            next: (data: Desk) => {
              this.desk = data;
              this.getColumns();
              this.getToDos(this.desk.id);
              this.name = new FormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
            },
            error: () => {
            }
          })
        }
      },
      error: () => {
      }
    });
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DeskCreationComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.desk = result;
        this.current_id = this.desk!.id;
        this.isLoaded = false;
        this.deskService.getDesks().subscribe({
          next: (data: Desk[]) => {
            this.isLoaded = true;
            this.desks = data;
            this.getColumns();
            this.name = new FormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
          },
          error: () => {
          }
        });
      }
    });

  }
  openTaskCreationDialog(todo: Todo): void {
    const dialogRefView = this.dialog.open(TaskCreationComponent, {
      data: {todo: todo, desk: this.desk, deskUser: this.getCurrentDesk()!.deskUser}
    });
    dialogRefView.afterClosed().subscribe( result => {
      let idx = this.toDos.findIndex(el => el.id === todo.id);
      console.log(idx);
      console.log(result);
      this.toDos[idx] = result;
      this.reloadTodosForColumns(this.toDos);
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
        this.columns.forEach(column => {
          this.isColumnDeleteLoaded.push(true);
        })

        this.reloadTodosForColumns(this.toDos);
      }
    })
  }
  closeDialog() {
    this.dialog.closeAll();
  }
  changeDesk(id: number) {
    this.current_id = id;
    this.opened = false;
    this.isLoaded = false;
    this.deskService.getDesk(id).subscribe({
      next: (data: Desk) => {
        this.desk = data;
        this.getColumns();
        this.getToDos(this.desk.id);
        this.name = new FormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
      },
      error: () => {
      }
    })
  }
  getDesk() : Desk {
    if (this.changed_index === -1) {
      this.desks.forEach( (el, index) => {
        if (el.deskUser.preference === 1) {
          this.index = index;
        }
      })

      return this.desks[this.index];
    }

    return this.desks[this.changed_index];
  }
  logout() {
    this.userService.logoutUser();
    this.router.navigate(['authorization']);
  }
  showDeskCreation() {
    this.openDialog();
    this.opened = false;
  }
  addToFavourite(index: number |undefined) {

    let founded = this.desks.find(el => el.deskUser.preference === 1);
    this.isFavouriteLoaded = false;
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
              this.isFavouriteLoaded = true;
              this.desk = data;
            },
            error: () => {
            }
          });
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
          this.deskService.getDesk(index).subscribe({
            next : (data: Desk) => {
              this.isFavouriteLoaded = true;
              this.desk = data;
            },
            error: () => {
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
      this.isNameUpdateLoaded = false;
      this.deskService.updateDesk(this.desk!.id, this.name.value).subscribe({
        next: (data: Desk) => {
          this.isNameUpdateLoaded = true;
          this.desk = data;
          let index = this.desks.findIndex(el => el.id === this.desk!.id);
          this.desks[index].name = data.name;
        }
      })
    }
  }

  getDeskOwner() {
    return this.desk?.deskUsers.find(el => el.role === 2);
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.current_id);
  }

  getColumns() {
    this.columnService.getColumns(this.desk!.id).subscribe({
      next: data => {
        this.isLoaded = true;
        this.isRemoveDeskLoaded = true;
        this.columns = data.sort(function (a, b) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.columns.forEach(column => {
          this.isColumnDeleteLoaded.push(true);
        })
        this.reloadTodosForColumns(this.toDos);
      },
      error: () => {
      }
    })
  }
  removeColumn(columnId: number) {
    this.isColumnDeleteLoaded[this.columns.findIndex(column => column.id === columnId)] = false;
    this.columnService.removeColumn(columnId).subscribe({
      next: data => {
        this.isColumnDeleteLoaded[this.columns.findIndex(column => column.id === columnId)] = true
        ;
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
      error: () => {
      }
    })
  }

  generateLink() {
    this.isLinkLoaded = false;
    this.deskService.setLink(this.desk!.id).subscribe( {
      next: data => {
        this.isLinkLoaded = true;
        this.desk = data;
      },
      error: () => {
      }
    })
  }

  hasInviteLink() {
    return !(this.desk?.inviteLink === null);
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
    this.isUserRemoveLoaded = false;
    this.deskService.removeUserFromDesk(usersId, this.desk!.id).subscribe({
      next: data => {
        this.isUserRemoveLoaded = true;
        this.desk = data;
      },
      error: () => {
      }
    })
  }

  removeDesk(deskId: number) {
    this.isRemoveDeskLoaded = false;
    this.deskService.removeDesk(deskId).subscribe({
      next: () => {
        this.loadDesks();
      },
      error: () => {
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
      },
      error: () => {
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

      }
    })
  }
  openToDoEditingDialog(todo: Todo) {
    const dialogRef = this.dialog.open(TodoEditingComponent, {
      data: {deskId: this.desk?.id, toDo: todo},
      width: '400px'
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result !== undefined) {
        this.toDos[this.toDos.findIndex(el => el.id === todo.id)] = result;
        this.reloadTodosForColumns(this.toDos);
      }
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
      }
    })
  }

  changeUserRole(event: MatSelectChange, deskUserId: number) {
    this.deskUserService.changeRole(deskUserId, event.value).subscribe({
      next: (data: DeskUsers[]) => {
        this.desk!.deskUsers = data.sort(function (a: DeskUsers, b: DeskUsers) {
          if (a.id > b.id) {
            return 1;
          }
          if (a.id < b.id) {
            return -1;
          }
          return 0;
        });
      },
      error: () => {
      }
    });
  }
  checkUserPermission(deskUser: DeskUsers, permissionName: string) {
    return this.rolesService.userHasPermission(deskUser, permissionName);
  }
  isUserAssigned(todo: Todo) {
    return !!todo.toDoUsers.find(el => el.deskUser.user.id === this.getCurrentUser().id && el.toDoUserType === 1);
  }

}

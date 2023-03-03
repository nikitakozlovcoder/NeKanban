import {ChangeDetectorRef, Component, OnChanges, OnInit} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskService} from "../../../services/desk.service";
import {UserService} from "../../../services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";
import {UntypedFormControl, Validators} from "@angular/forms";
import {CdkDragDrop, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import {TodoShowComponent} from "../../todo/todo-show/todo-show.component";
import {Column} from "../../../models/column";
import {ColumnCreationComponent} from "../../column/column-creation/column-creation.component";
import {ColumnService} from "../../../services/column.service";
import {Todo} from "../../../models/todo";
import {TodoService} from "../../../services/todo.service";
import {TodoCreationComponent} from "../../todo/todo-creation/todo-creation.component";
import {TodoEditingComponent} from "../../todo/todo-editing/todo-editing.component";
import {ColumnUpdatingComponent} from "../../column/column-updating/column-updating.component";
import {MatSelectChange} from "@angular/material/select";
import {RolesService} from "../../../services/roles.service";
import {DeskUser} from "../../../models/deskUser";
import {DeskRole} from "../../../models/deskrole";
import {Role} from "../../../models/Role";
import {DeskUserService} from "../../../services/deskUser.service";
import {MatSnackBar} from "@angular/material/snack-bar";

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
  changedIndex: number = -1;
  currentId: number = -1;
  columns: Column[] = [];
  toDos: Todo[] = [];
  clientBaseHref = "";
  isLoaded = false;
  isNameUpdateLoaded = true;
  isColumnDeleteLoaded: boolean[] = [];
  isLinkLoaded = true;
  isRemoveDeskLoaded = true;
  isUserRemoveLoaded = true;
  isFavouriteLoaded = true;
  roles : Role[] = [];

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

  constructor(private deskService: DeskService,
              private userService: UserService,
              private router: Router,
              public dialog: MatDialog,
              private columnService: ColumnService,
              private todoService: TodoService,
              private rolesService: RolesService,
              private deskUserService: DeskUserService,
              public snackBar: MatSnackBar,
              private route: ActivatedRoute) {
    this.opened = false;
    route.params.subscribe(params => {
      if (params['id'] === undefined) {
        this.deskService.getDesks().subscribe(result => {
          if (result.length == 0) {
            this.desks = result;
            this.isLoaded = true;
            return;
          }
          let founded = result.find(el => el.deskUser.preference === 1);

          if (founded != undefined) {
            this.router.navigate(['/desks', founded.id]);
          }
          else {
            this.router.navigate(['/desks', result[0].id]);
          }
        });
      }
      else {
        this.loadDesks(parseInt(params['id']));
      }
    });
  }

  ngOnInit(): void {
    this.clientBaseHref = window.location.href;
  }
  /*ngAfterViewInit() {
    this.cdr.detectChanges();
  }*/
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(6)]);
  loadDesks(deskId: number) {
    this.isLoaded = false;
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {

        this.desks = data;
        if (this.desks.length === 0){
          this.isLoaded = true;
          return;
        }
        if (!this.desks.some(el => el.id === deskId)) {
          this.router.navigate(['/**'], { skipLocationChange: true });
          return;
        }
        this.deskService.getDesk(deskId).subscribe( {
          next: (data: Desk) => {
            this.desk = data;
            this.getColumns();
            this.name = new UntypedFormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
          }
        });
      },
      error: () => {
      }
    });
  }

  openTaskShowingDialog(todo: Todo): void {
    const dialogRefView = this.dialog.open(TodoShowComponent, {
      data: {todo: todo, desk: this.desk, deskUser: this.getCurrentDesk()!.deskUser, roles: this.roles},
      panelClass: 'todo-show-wrap'
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

  getDeskOwner() {
    return this.desk?.deskUsers.find(el => el.isOwner);
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.desk?.id);
  }

  getColumns() {
    this.columnService.getColumns(this.desk!.id).subscribe({
      next: data => {
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
      ,complete: () => this.getToDos(this.desk!.id)
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

  getCurrentUser() {
    return JSON.parse(localStorage.getItem("currentUser")!);
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
      },
      complete: () => this.initRolesForDesk()
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

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  isUserAssigned(todo: Todo) {
    return !!todo.toDoUsers.find(el => el.deskUser.user.id === this.getCurrentUser().id && el.toDoUserType === 1);
  }

  private initRolesForDesk() {
    this.rolesService.getRoles(this.desk!.id).subscribe(result => {
      this.roles = result;
      this.isLoaded = true;
    });
  }

}

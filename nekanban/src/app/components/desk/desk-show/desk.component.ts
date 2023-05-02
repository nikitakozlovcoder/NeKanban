import {Component, OnInit} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskService} from "../../../services/desk.service";
import {UserService} from "../../../services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
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
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {DeskUserService} from "../../../services/deskUser.service";
import { BehaviorSubject, combineLatest, map } from "rxjs";
import {UserStorageService} from "../../../services/userStorage.service";

@Component({
  selector: 'app-desk',
  templateUrl: './desk.component.html',
  styleUrls: ['./desk.component.css']
})
export class DeskComponent implements OnInit {
  opened: boolean = false;
  desks: Desk[] = [];
  desk: Desk | undefined;
  index: number = 0;
  columns: Column[] = [];
  toDos: Todo[] = [];
  isColumnDeleteLoaded: boolean[] = [];
  roles : Role[] = [];
  desksLoaded = new BehaviorSubject(false);
  deskLoaded = new BehaviorSubject(false);
  columnsLoaded = new BehaviorSubject(false);
  todosLoaded = new BehaviorSubject(false);
  rolesLoaded = new BehaviorSubject(false);

  get isLoaded() {
    return combineLatest([this.desksLoaded, this.columnsLoaded, this.todosLoaded, this.rolesLoaded, this.deskLoaded])
      .pipe(map(x => x.every(isLoaded => isLoaded)));
  }

  constructor(private readonly deskService: DeskService,
              private readonly userService: UserService,
              private readonly router: Router,
              public readonly dialog: MatDialog,
              private readonly columnService: ColumnService,
              private readonly todoService: TodoService,
              public readonly rolesService: RolesService,
              public readonly deskUserService: DeskUserService,
              private readonly route: ActivatedRoute,
              private readonly userStorageService: UserStorageService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id'] === undefined) {
        this.deskService.getDesks().subscribe(result => {
          if (result.length == 0) {
            this.desks = result;
            this.desksLoaded.next(true);
            return;
          }

          let founded = result.find(el => el.deskUser.preference === 1);
          if (founded != undefined) {
            this.router.navigate(['/desks', founded.id]).then();
          }
          else {
            this.router.navigate(['/desks', result[0].id]).then();
          }
        });
      }
      else {
        this.loadDesks(parseInt(params['id']));
      }
    });
  }

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
        if (event.currentIndex === event.container.data.length) {
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

  dropColumn(event: CdkDragDrop<Column[]>) {
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


  loadDesks(deskId: number) {
    this.desksLoaded.next(false);
    this.deskService.getDesks().subscribe({
      next: (data: Desk[]) => {

        this.desks = data;
        if (this.desks.length === 0) {
          this.desksLoaded.next(true);
          return;
        }
        if (!this.desks.some(el => el.id === deskId)) {
          this.router.navigate(['/**'], {skipLocationChange: true}).then();
          return;
        }
        this.desksLoaded.next(true);
      },
      error: () => {
      }
    });
    this.loadDesk(deskId);
    this.getColumns(deskId);
    this.getToDos(deskId);
    this.initRolesForDesk(deskId);
  }

  loadDesk(deskId: number) {
    this.deskLoaded.next(false);
    this.deskService.getDesk(deskId).subscribe( {
      next: (data: Desk) => {
        this.desk = data;
        this.deskLoaded.next(true);
      }
    });
  }

  openTaskShowingDialog(todo: Todo): void {
    const dialogRefView = this.dialog.open(TodoShowComponent, {
      data: {todoId: todo.id, desk: this.desk, deskUser: this.getCurrentDesk()!.deskUser, roles: this.roles},
      panelClass: 'todo-show-wrap'
    });
    dialogRefView.afterClosed().subscribe( result => {
      let idx = this.toDos.findIndex(el => el.id === todo.id);
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
        this.columns.forEach(() => this.isColumnDeleteLoaded.push(true))

        this.reloadTodosForColumns(this.toDos);
      }
    })
  }

  getCurrentDesk() {
    return this.desks.find(el => el.id === this.desk?.id);
  }

  getColumns(deskId: number) {
    this.columnsLoaded.next(false);
    this.columnService.getColumns(deskId).subscribe({
      next: data => {
        this.columnsLoaded.next(true);
        this.columns = data.sort(function (a, b) {
          if (a.order > b.order) {
            return 1;
          }
          if (a.order < b.order) {
            return -1;
          }
          return 0;
        });
        this.columns.forEach(() => {
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
        this.isColumnDeleteLoaded[this.columns.findIndex(column => column.id === columnId)] = true;
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
    return this.userStorageService.getUserFromStorage();
  }

  getToDos(deskId: number) {
    this.todosLoaded.next(false);
    this.todoService.getToDos(deskId).subscribe({
      next: data => {
        this.toDos = data;
        this.reloadTodosForColumns(this.toDos);
        /*for (let i = 0; i < this.columns.length; i++) {
          this.columns[i].todos =  data.filter( todo => todo.column.id === this.columns[i].id).sort(function (a: Todo, b: Todo) {
            if (a.order > b.order) {
              return 1;
            }
            if (a.order < b.order) {
              return -1;
            }
            return 0;
          });
        }*/
        this.todosLoaded.next(true);
      },
      error: () => {
      }
    })
  }

  openToDoCreationDialog() {
    const dialogRef = this.dialog.open(TodoCreationComponent, {
      data: {deskId: this.desk?.id, isEdit: false},
      width: '600px'
    });
    dialogRef.afterClosed().subscribe( result => {
      if (result != undefined) {
        this.toDos.push(result);
        this.reloadTodosForColumns(this.toDos);
        /*this.toDos = result;
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
        }*/

      }
    })
  }
  openToDoEditingDialog(todo: Todo) {
    const dialogRef = this.dialog.open(TodoEditingComponent, {
      data: {deskId: this.desk?.id, toDo: todo},
      width: '600px'
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

  isUserAssigned(todo: Todo) {
    return !!todo.toDoUsers.find(el => el.deskUser.user.id === this.getCurrentUser().id && el.toDoUserType === 1);
  }

  private initRolesForDesk(deskId: number) {
    this.rolesLoaded.next(false);
    this.rolesService.getRoles(deskId).subscribe(result => {
      this.roles = result;
      this.rolesLoaded.next(true);
    });
  }

}

import {AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {
  UntypedFormControl,
} from "@angular/forms";
import {Desk} from "../../../models/desk";
import {User} from "../../../models/user";
import {MatSelect} from "@angular/material/select";
import {TodoService} from "../../../services/todo.service";
import {RolesService} from "../../../services/roles.service";
import {DeskUser} from "../../../models/deskUser";
import {Role} from "../../../models/Role";
import {BehaviorSubject} from "rxjs";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-task-creation',
  templateUrl: './todo-show.component.html',
  styleUrls: ['./todo-show.component.css']
})
export class TodoShowComponent implements OnInit {

  todo?: Todo;
  usersSelected : number[] = [];
  userSelected : number[] = [];
  users = new UntypedFormControl(this.usersSelected);
  user = new UntypedFormControl(this.userSelected);
  areUsersLoaded = new BehaviorSubject(true);
  todoLoaded = new BehaviorSubject(false);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todoId: number, isEdit: boolean, desk: Desk, deskUser: DeskUser, roles: Role[]},
              private readonly toDoService: TodoService,
              public readonly rolesService: RolesService,
              public dialogRef: MatDialogRef<TodoShowComponent>) {
    this.dialogRef.beforeClosed().subscribe(() => this.closeDialog());
  }

  ngOnInit(): void {
    this.toDoService.getToDo(this.data.todoId).subscribe({
      next: value => {
        this.todo = value;
        this.todoLoaded.next(true);
        this.usersSelected = this.getIdsOfSelectedUsers();
        this.users.patchValue(this.usersSelected);
        this.userSelected = this.getIdOfSingleUser();
        this.user.patchValue(this.userSelected);
      }
    })
  }

  closeDialog() {
    this.dialogRef.close(this.todo!);
  }

  getToDoCreator() {
    return this.todo!.toDoUsers.find(el => el.toDoUserType == 0);
  }

  getIdOfSingleUser() {
    let ids : number[] = [];
    let founded = this.getToDoUsers().find(el => el.deskUser.user.id === this.data.deskUser.user.id);
    if (founded !== undefined) {
      ids.push(founded.deskUser.user.id);
    }
    return ids;
  }

  getToDoUsers() {
    return this.todo!.toDoUsers.filter(el => el.toDoUserType != 0);
  }

  getDeskUsers() : User[] {
    let deskUsers: User[] = [];
    this.data.desk.deskUsers.forEach( el => {
      deskUsers.push(el.user);
    })
    return deskUsers;
  }

  getAllTodoUsers() : User[] {
    let todoUsers : User[] = [];
    this.todo!.toDoUsers.forEach( el => {
      todoUsers.push(el.deskUser.user);
    })
    return todoUsers;
  }

  getIdsOfSelectedUsers() : number[] {
    let selectedUsers : User[] = this.getDeskUsers().filter(el => this.getAllTodoUsers().some(someEl => someEl.id === el.id)
      && this.todo!.toDoUsers.find(todoUser => todoUser.deskUser.user.id === el.id  && todoUser.toDoUserType != 0));
    let ids : number[] = [];
    selectedUsers.forEach( el => {
      ids.push(el.id);
    })
    return ids;
  }

  changeUsers(select:MatSelect)  {
    let newIds : number[] = select.value;
    let appearedIds: number[] = [];
    newIds.forEach(el => {
      if (!this.usersSelected.includes(el)) {
        appearedIds.push(el);
      }
    })
    let disappearedIds : number[] = [];
    this.usersSelected.forEach( el => {
      if (!newIds.includes(el)) {
        disappearedIds.push(el);
      }
    })

    appearedIds.forEach(el => {
      this.areUsersLoaded.next(false);
      this.dialogRef.disableClose = true;
      let deskUser = this.data.desk.deskUsers.find(obj => obj.user.id === el);
      this.toDoService.assignUser(this.data.todoId, deskUser!.id).subscribe({
        next: data => {
          if (appearedIds.indexOf(el) === appearedIds.length - 1) {
            this.areUsersLoaded.next(true);
            this.dialogRef.disableClose = false;
          }
          this.todo!.toDoUsers = data.toDoUsers;
        },
        error: _ => {
        }
      })
    })
    disappearedIds.forEach( el => {
      this.areUsersLoaded.next(false);
      this.dialogRef.disableClose = true;
      let todo = this.todo!.toDoUsers.find(obj => obj.deskUser.user.id === el && obj.toDoUserType != 0);
      this.toDoService.removeUser(todo!.id).subscribe({
        next: data => {
          if (disappearedIds.indexOf(el) === disappearedIds.length - 1) {
            this.areUsersLoaded.next(true);
            this.dialogRef.disableClose = false;
          }
          this.todo!.toDoUsers = data.toDoUsers;
        },
        error: _ => {
        }
      })
    })
    this.usersSelected = newIds;
    this.users.patchValue(this.usersSelected);
  }

  changeSingleUser(select:MatSelect) {
    let newIds : number[] = select.value;
    if (newIds.length === 0) {
      let todo = this.todo!.toDoUsers.find(obj => obj.deskUser.user.id === this.data.deskUser.user.id);
      if (todo !== undefined) {
        this.areUsersLoaded.next(false);
        this.dialogRef.disableClose = true;
        this.toDoService.removeUser(todo.id).subscribe({
          next: data => {
            this.areUsersLoaded.next(true);
            this.todo!.toDoUsers = data.toDoUsers;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }

    }
    else {
      if (!this.usersSelected.includes(this.data.deskUser.user.id)) {
        this.areUsersLoaded.next(false);
        this.dialogRef.disableClose = true;
        this.toDoService.assignUser(this.data.todoId, this.data.deskUser.id).subscribe({
          next: data => {
            this.areUsersLoaded.next(true);
            this.todo!.toDoUsers = data.toDoUsers;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }
    }
  }
}

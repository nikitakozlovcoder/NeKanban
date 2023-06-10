import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {Desk} from "../../../models/desk";
import {TodoService} from "../../../services/todo.service";
import {RolesService} from "../../../services/roles.service";
import {DeskUser} from "../../../models/deskUser";
import {Role} from "../../../models/Role";
import {BehaviorSubject} from "rxjs";
import {ToDoUserType} from "../../../constants/ToDoUserType";

@Component({
  selector: 'app-task-creation',
  templateUrl: './todo-show.component.html',
  styleUrls: ['./todo-show.component.css']
})
export class TodoShowComponent implements OnInit {

  todo?: Todo;
  todoLoaded = new BehaviorSubject(false);
  newAssigneeLoaded = new BehaviorSubject(true);
  assigneesLoaded : BehaviorSubject<boolean>[] = [];
  get freeUsers() {
    if (this.rolesService.userHasPermission(this.data.roles, this.data.deskUser, this.rolesService.permissionsTypes.ManageAssigners)) {
      return this.data.desk.deskUsers.filter(x => !this.todo!.toDoUsers.some(el => el.deskUser.id === x.id && el.toDoUserType !== ToDoUserType.Creator));
    }
    else if (this.rolesService.userHasPermission(this.data.roles, this.data.deskUser, this.rolesService.permissionsTypes.AssignTasksThemself)) {
      return this.todo!.toDoUsers.some(x => x.deskUser.id === this.data.deskUser.id) ? [] : [this.data.deskUser];
    }
    return [];
  }

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
        this.todo.toDoUsers.forEach(() => this.assigneesLoaded.push(new BehaviorSubject<boolean>(true)));
      }
    })
  }

  closeDialog() {
    this.dialogRef.close(this.todo!);
  }

  getToDoCreator() {
    return this.todo!.toDoUsers.find(el => el.toDoUserType == 0);
  }
  getToDoUsers() {
    return this.todo!.toDoUsers.filter(el => el.toDoUserType != 0);
  }

  assignUser(deskUserId: number) {
    this.newAssigneeLoaded.next(false);
    this.toDoService.assignUser(this.data.todoId, deskUserId).subscribe({
      next: data => {
        this.newAssigneeLoaded.next(true);
        this.todo!.toDoUsers = data.toDoUsers;
        this.dialogRef.disableClose = false;
      },
      error: _ => {
      }
    }).add(() => {
      this.assigneesLoaded = [];
      this.todo!.toDoUsers.forEach(() => this.assigneesLoaded.push(new BehaviorSubject<boolean>(true)));
    })
  }

  removeUser(todoUserId: number) {
    this.assigneesLoaded[this.todo!.toDoUsers.findIndex(x => x.id === todoUserId) - 1].next(false);
    this.toDoService.removeUser(todoUserId).subscribe({
      next: data => {
        this.todo!.toDoUsers = data.toDoUsers;
        this.dialogRef.disableClose = false;
      },
      error: _ => {
      }
    }).add(() => {
      this.assigneesLoaded = [];
      this.todo!.toDoUsers.forEach(() => this.assigneesLoaded.push(new BehaviorSubject<boolean>(true)));
    })
  }
}

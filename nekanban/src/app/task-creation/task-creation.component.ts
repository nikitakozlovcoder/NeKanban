import {Component, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../models/todo";
import {FormControl, Validators} from "@angular/forms";
import {Desk} from "../models/desk";
import {User} from "../models/user";
import {MatSelect, MatSelectChange} from "@angular/material/select";
import {TodoService} from "../services/todo.service";
import {RolesService} from "../services/roles.service";
import {DeskUsers} from "../models/deskusers";

@Component({
  selector: 'app-task-creation',
  templateUrl: './task-creation.component.html',
  styleUrls: ['./task-creation.component.css']
})
export class TaskCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todo: Todo, isEdit: boolean, desk: Desk, deskUser: DeskUsers}, private toDoService: TodoService, private rolesService: RolesService, public dialogRef: MatDialogRef<TaskCreationComponent>) {
    this.dialogRef.beforeClosed().subscribe(() => this.closeDialog());
  }

  usersSelected : number[] = this.getIdsOfSelectedUsers();

  users = new FormControl(this.usersSelected);


  ngOnInit(): void {
    console.log(this.usersSelected);
  }

  closeDialog() {
    console.log("Fired");
    this.dialogRef.close(this.data.todo);
  }
  getToDoCreator() {
    return this.data.todo.toDoUsers.find(el => el.toDoUserType == 0);
  }

  getToDoUsers() {
    return this.data.todo.toDoUsers.filter(el => el.toDoUserType != 0);
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
    this.data.todo.toDoUsers.forEach( el => {
      todoUsers.push(el.deskUser.user);
    })
    return todoUsers;
  }
  getIdsOfSelectedUsers() : number[] {
    let selectedUsers : User[] = this.getDeskUsers().filter(el => this.getAllTodoUsers().some(someEl => someEl.id === el.id) && this.data.todo.toDoUsers.find(todoUser => todoUser.deskUser.user.id === el.id  && todoUser.toDoUserType != 0));
    console.log(selectedUsers);
    let ids : number[] = [];
    selectedUsers.forEach( el => {
      ids.push(el.id);
    })
    return ids;
  }
  changeUsers(select:MatSelect)  {
    let newIds : number[] = select.value;
    console.log("New ids:");
    console.log(newIds);
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
      let deskUser = this.data.desk.deskUsers.find(obj => obj.user.id === el);
      this.toDoService.assignUser(this.data.todo.id, deskUser!.id).subscribe({
        next: data => {
          this.data.todo = data;
        },
        error: err => {
          console.log(err);
        }
      })
    })
    disappearedIds.forEach( el => {
      let todo = this.data.todo.toDoUsers.find(obj => obj.deskUser.user.id === el && obj.toDoUserType != 0);
      this.toDoService.removeUser(todo!.id).subscribe({
        next: data => {
          this.data.todo = data;
        },
        error: err => {
          console.log(err);
        }
      })
    })
    this.usersSelected  = newIds;
    console.log("Appeared ids: ");
    console.log(appearedIds);
    console.log("Disappeared ids: ");
    console.log(disappearedIds);
    console.log(select.value);
  }
  checkUserPermission(deskUser: DeskUsers, permissionName: string) {
    return this.rolesService.userHasPermission(deskUser, permissionName);
  }
  changeSingleUser(select:MatSelect) {
    let newIds : number[] = select.value;
    if (newIds.length === 0) {
      let todo = this.data.todo.toDoUsers.find(obj => obj.deskUser.user.id === this.data.deskUser.user.id);
      this.toDoService.removeUser(todo!.id).subscribe({
        next: data => {
          this.data.todo = data;
        },
        error: err => {
          console.log(err);
        }
      })
    }
    else {
      if (!this.usersSelected.includes(this.data.deskUser.user.id)) {
        this.toDoService.assignUser(this.data.todo.id, this.data.deskUser.id).subscribe({
          next: data => {
            this.data.todo = data;
          },
          error: err => {
            console.log(err);
          }
        })
      }

    }
  }

}

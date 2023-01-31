import {Component, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {UntypedFormControl, Validators} from "@angular/forms";
import {Desk} from "../../../models/desk";
import {User} from "../../../models/user";
import {Comment} from "../../../models/comment";
import {MatSelect, MatSelectChange} from "@angular/material/select";
import {TodoService} from "../../../services/todo.service";
import {RolesService} from "../../../services/roles.service";
import {DeskUsers} from "../../../models/deskusers";
import {DataGeneratorService} from "../../../services/dataGenerator.service";
import {Column} from "../../../models/column";

@Component({
  selector: 'app-task-creation',
  templateUrl: './todo-show.component.html',
  styleUrls: ['./todo-show.component.css']
})
export class TodoShowComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todo: Todo, isEdit: boolean, desk: Desk, deskUser: DeskUsers}, private toDoService: TodoService, private rolesService: RolesService, public dialogRef: MatDialogRef<TodoShowComponent>, private dataGeneratorService: DataGeneratorService) {
    this.dialogRef.beforeClosed().subscribe(() => this.closeDialog());
  }


  usersSelected : number[] = this.getIdsOfSelectedUsers();
  userSelected : number[] = this.getIdOfSingleUser();
  users = new UntypedFormControl(this.usersSelected);
  user = new UntypedFormControl(this.userSelected);
  isLoaded = true;
  isSortDescending = true;
  comments: Comment[] = this.sortComments(this.dataGeneratorService.generateComments());

  commentInput = new UntypedFormControl('', [Validators.required, Validators.minLength(5)]);


  ngOnInit(): void {
  }

  closeDialog() {
    this.dialogRef.close(this.data.todo);
  }
  getToDoCreator() {
    return this.data.todo.toDoUsers.find(el => el.toDoUserType == 0);
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
      this.isLoaded = false;
      this.dialogRef.disableClose = true;
      let deskUser = this.data.desk.deskUsers.find(obj => obj.user.id === el);
      this.toDoService.assignUser(this.data.todo.id, deskUser!.id).subscribe({
        next: data => {
          if (appearedIds.indexOf(el) === appearedIds.length - 1) {
            this.isLoaded = true;
            this.dialogRef.disableClose = false;
          }
          this.data.todo = data;
        },
        error: _ => {
        }
      })
    })
    disappearedIds.forEach( el => {
      this.isLoaded = false;
      this.dialogRef.disableClose = true;
      let todo = this.data.todo.toDoUsers.find(obj => obj.deskUser.user.id === el && obj.toDoUserType != 0);
      this.toDoService.removeUser(todo!.id).subscribe({
        next: data => {
          if (disappearedIds.indexOf(el) === disappearedIds.length - 1) {
            this.isLoaded = true;
            this.dialogRef.disableClose = false;
          }
          this.data.todo = data;
        },
        error: _ => {
        }
      })
    })
    this.usersSelected  = newIds;
  }
  checkUserPermission(deskUser: DeskUsers, permissionName: string) {
    return this.rolesService.userHasPermission(deskUser, permissionName);
  }
  changeSingleUser(select:MatSelect) {
    let newIds : number[] = select.value;
    console.log(select.value);
    if (newIds.length === 0) {
      let todo = this.data.todo.toDoUsers.find(obj => obj.deskUser.user.id === this.data.deskUser.user.id);
      if (todo !== undefined) {
        this.isLoaded = false;
        this.dialogRef.disableClose = true;
        this.toDoService.removeUser(todo.id).subscribe({
          next: data => {
            this.isLoaded = true;
            this.data.todo = data;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }

    }
    else {
      if (!this.usersSelected.includes(this.data.deskUser.user.id)) {
        this.isLoaded = false;
        this.dialogRef.disableClose = true;
        this.toDoService.assignUser(this.data.todo.id, this.data.deskUser.id).subscribe({
          next: data => {
            this.isLoaded = true;
            this.data.todo = data;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }
    }
  }
  sortComments(comments: Comment[]): Comment[] {
    if (this.isSortDescending) {
      return comments.sort(function (a: Comment, b: Comment) {
        if (a.datetime > b.datetime) {
          return -1;
        }
        if (a.datetime < b.datetime) {
          return 1;
        }
        return 0;
      });
    }
    return comments.sort(function (a: Comment, b: Comment) {
      if (a.datetime > b.datetime) {
        return 1;
      }
      if (a.datetime < b.datetime) {
        return -1;
      }
      return 0;
    });
  }
  toggleCommentsOrder() {
    this.isSortDescending = !this.isSortDescending;
    this.comments = this.sortComments(this.comments);
  }
}

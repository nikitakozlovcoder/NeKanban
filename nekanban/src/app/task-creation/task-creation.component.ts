import {Component, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Todo} from "../models/todo";
import {FormControl, Validators} from "@angular/forms";
import {Desk} from "../models/desk";
import {User} from "../models/user";
import {MatSelect, MatSelectChange} from "@angular/material/select";
import {TodoService} from "../services/todo.service";
import {RolesService} from "../services/roles.service";
import {DeskUsers} from "../models/deskusers";
interface Topping {
  name: string;
  price: number;
  id: number;
}
@Component({
  selector: 'app-task-creation',
  templateUrl: './task-creation.component.html',
  styleUrls: ['./task-creation.component.css']
})
export class TaskCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todo: Todo, isEdit: boolean, desk: Desk, deskUser: DeskUsers}, private toDoService: TodoService, private rolesService: RolesService) { }

  employess : string[] = ['ivan', 'petr', 'konstantin', 'adam', 'ivan', 'petr', 'konstantin', 'adam']

  selectedToppingList: Topping[] = [
    {name: "Apple", price: 2000, id: 1}
  ];
  toppingList: Topping[] = [
    {name: "Apple", price: 2000, id: 1},
    {name: "Banana", price: 300, id: 2},
    {name: "Potato", price: 500, id: 3}
  ];
  //selectedToppingList: string[] = ['Mushroom', 'Onion', 'Tomato'];
  toppings = new FormControl(this.selectedToppingList, Validators.required);

  //toppingList: string[] = ['Extra cheese', 'Mushroom', 'Onion', 'Pepperoni', 'Sausage', 'Tomato'];

  usersSelected : number[] = this.getIdsOfSelectedUsers();

  users = new FormControl(this.usersSelected, Validators.required);


  ngOnInit(): void {
    /*console.log(this.getDeskUsers());
    console.log(this.getAllTodoUsers());
    let testUser : User | undefined = this.getDeskUsers().find(el => el.name === "Name");
    console.log("Finded ");
    console.log(testUser);
    console.log(this.getAllTodoUsers().includes(testUser!));*/
    console.log(this.usersSelected);
  }
  getSelectedUsers() {

  }
  getToDoCreator() {
    return this.data.todo.toDoUsers.find(el => el.toDoUserType == 0);
  }

  getToDoUsers() {
    return this.data.todo.toDoUsers.filter(el => el.toDoUserType != 0);
  }
  getDeskUsers() : User[] {
    let deskUsers: User[] = [];
    let toDoCreator = this.getToDoCreator()!.deskUser.user;
    this.data.desk.deskUsers.forEach( el => {
      if (el.user.id !== toDoCreator.id) {
        deskUsers.push(el.user);
      }
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
    let selectedUsers : User[] = this.getDeskUsers().filter(el => this.getAllTodoUsers().some(someEl => someEl.id === el.id));
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
      let todo = this.data.todo.toDoUsers.find(obj => obj.deskUser.user.id === el);
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
      this.toDoService.removeUser(this.data.deskUser.id).subscribe({
        next: data => {
          this.data.todo = data;
        },
        error: err => {
          console.log(err);
        }
      })
    }
    else {
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

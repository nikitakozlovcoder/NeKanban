import {Todo} from "./todo";
import {User} from "./user";

export class Comment {
  id: number;
  text: string;
  user: User;
  //todo: Todo;
  datetime: Date;
  constructor(id: number, text: string, user: User, datetime: Date) {
    this.id = id;
    this.text = text;
    this.user = user;
    //this.todo = todo;
    this.datetime = datetime;
  }
}

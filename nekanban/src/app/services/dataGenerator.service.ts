import {Injectable} from "@angular/core";
import {Comment} from "../models/comment";
import {User} from "../models/user";
import {TodoService} from "./todo.service";
import {Todo} from "../models/todo";

@Injectable()
export class DataGeneratorService {
  constructor(private todoService: TodoService) {
  }
  private randomDate(start : Date, end: Date, startHour: number, endHour: number) {
    let date = new Date(+start + Math.random() * (end.getTime() - start.getTime()));
    let hour = startHour + Math.random() * (endHour - startHour) | 0;
    date.setHours(hour);
    return date;
  }
}

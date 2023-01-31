import {Injectable} from "@angular/core";
import {Comment} from "../models/comment";
import {User} from "../models/user";
import {TodoService} from "./todo.service";
import {Todo} from "../models/todo";

@Injectable()
export class DataGeneratorService {
  constructor(private todoService: TodoService) {
  }
  generateComments() : Comment[] {
    return [new Comment(1, "hihihihi", new User("test", "test", "test@test.test", "qwqqwqw", 1), new Date()),
      new Comment(2, "22e2e", new User("test", "test", "test@test.test", "231232", 1), this.randomDate(new Date(2020, 1, 1), new Date(), 0, 24)),
      new Comment(3, "wqewqewqewqe", new User("test", "test", "test@test.test", "qwqqwqw", 1), this.randomDate(new Date(2020, 1, 1), new Date(), 0, 24)),
      new Comment(4, "1111111111", new User("test", "test", "test@test.test", "qwqqwqw", 1), this.randomDate(new Date(2020, 1, 1), new Date(), 0, 24)),
      new Comment(5, "ereerererssdsdsdsdsdsdsdsd", new User("test", "test", "test@test.test", "qwqqwqw", 1), this.randomDate(new Date(2020, 1, 1), new Date(), 0, 24))];
  }
  private randomDate(start : Date, end: Date, startHour: number, endHour: number) {
    var date = new Date(+start + Math.random() * (end.getTime() - start.getTime()));
    var hour = startHour + Math.random() * (endHour - startHour) | 0;
    date.setHours(hour);
    return date;
  }
}

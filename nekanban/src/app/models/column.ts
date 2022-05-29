import {Todo} from "./todo";

export class Column {
  id: number;
  name: string;
  type: number;
  typename: string;
  order: number;
  todos: Todo[] = [];
  constructor(id: number, name: string, type: number, typename: string, order: number) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.typename = typename;
    this.order = order;
  }
}

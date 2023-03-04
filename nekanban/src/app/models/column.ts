import {Todo} from "./todo";

export interface Column {
  id: number;
  name: string;
  type: number;
  typename: string;
  order: number;
  todos: Todo[];
}

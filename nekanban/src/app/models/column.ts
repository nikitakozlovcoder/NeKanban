export class Column {
  id: number;
  name: string;
  type: number;
  typename: string;
  order: number;
  constructor(id: number, name: string, type: number, typename: string, order: number) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.typename = typename;
    this.order = order;
  }
}

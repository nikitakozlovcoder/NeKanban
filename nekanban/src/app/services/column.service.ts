import {Injectable} from "@angular/core";
import {AppHttpService} from "./app-http.service";
import {Column} from "../models/column";

@Injectable()
export class ColumnService {
  constructor(private httpService: AppHttpService) { }

  getColumns(deskId: number) {
    return this.httpService.get<Column[]>("Columns/GetColumns/" + deskId);
  }

  addColumn(deskId: number, name: string) {
    return this.httpService.post<Column[]>("Columns/CreateColumn/" + deskId, {name});
  }

  removeColumn(columnId: number) {
    return this.httpService.delete<Column[]>("Columns/DeleteColumn/" + columnId);
  }

  moveColumn(columnId: number, position: number) {
    return this.httpService.put<Column[]>("Columns/MoveColumn/" + columnId, {position});
  }

  updateColumn(columnId: number, name: string) {
    return this.httpService.put<Column[]>("Columns/UpdateColumn/" + columnId, {name});
  }
}

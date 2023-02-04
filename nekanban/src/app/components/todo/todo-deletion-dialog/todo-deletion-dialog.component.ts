import {Component, OnInit} from '@angular/core';
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";

@Component({
  selector: 'app-todo-deletion-dialog',
  templateUrl: './todo-deletion-dialog.component.html',
  styleUrls: ['./todo-deletion-dialog.component.css']
})
export class TodoDeletionDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<TodoDeletionDialogComponent>) { }

  ngOnInit(): void {
  }

  closeWithAccept() {
    this.dialogRef.close(DialogActionTypes.Accept);
  }
  closeWithReject() {
    this.dialogRef.close(DialogActionTypes.Reject);
  }
}

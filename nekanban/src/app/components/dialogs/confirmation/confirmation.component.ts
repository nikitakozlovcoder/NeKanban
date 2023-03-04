import { Component, OnInit } from '@angular/core';
import {MatDialogRef} from "@angular/material/dialog";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ConfirmationComponent>) { }

  ngOnInit(): void {
  }

  closeWithAccept() {
    this.dialogRef.close(DialogActionTypes.Accept);
  }
  closeWithReject() {
    this.dialogRef.close(DialogActionTypes.Reject);
  }

}

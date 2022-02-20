import { Component, OnInit } from '@angular/core';
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {DeskCreationComponent} from "../desk-creation/desk-creation.component";

@Component({
  selector: 'app-desk-creation-dialog',
  templateUrl: './desk-creation-dialog.component.html',
  styleUrls: ['./desk-creation-dialog.component.css']
})
export class DeskCreationDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeskCreationDialogComponent>) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

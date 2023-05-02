import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import ErrorTypes from "../constants/ErrorTypes";

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
  errorsEnum = ErrorTypes;
  constructor(@Inject(MAT_DIALOG_DATA) public data:{errorType: ErrorTypes}) { }

  ngOnInit(): void {
  }

}

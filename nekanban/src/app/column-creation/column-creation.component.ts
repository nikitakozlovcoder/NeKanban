import {Component, Inject, OnInit} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {Desk} from "../models/desk";
import {DeskService} from "../services/desk.service";
import {Router} from "@angular/router";
import {DeskComponent} from "../desk/desk.component";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ColumnService} from "../services/column.service";
import {Column} from "../models/column";

@Component({
  selector: 'app-column-creation',
  templateUrl: './column-creation.component.html',
  styleUrls: ['./column-creation.component.css']
})
export class ColumnCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number}, private deskService: DeskService, private router: Router, private deskComponent: DeskComponent, public dialogRef: MatDialogRef<ColumnCreationComponent>, private columnService: ColumnService) { }

  ngOnInit(): void {
  }
  name = new FormControl('', [Validators.required, Validators.minLength(3)]);

  createColumn() {
    this.columnService.addColumn(this.data.deskId, this.name.value).subscribe({
      next: (data: Column[]) => {
        this.dialogRef.close(data);
      }
    });
  }

}

import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../models/todo";
import {Column} from "../models/column";
import {FormControl, Validators} from "@angular/forms";
import {ColumnService} from "../services/column.service";

@Component({
  selector: 'app-column-updating',
  templateUrl: './column-updating.component.html',
  styleUrls: ['./column-updating.component.css']
})
export class ColumnUpdatingComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {column: Column}, private columnService: ColumnService, public dialogRef: MatDialogRef<ColumnUpdatingComponent>) { }

  ngOnInit(): void {
  }
  name = new FormControl(this.data.column.name, [Validators.required, Validators.minLength(8)]);

  updateColumn() {
    this.columnService.updateColumn(this.data.column.id, this.name.value).subscribe({
      next: (data: Column[]) => {
        this.dialogRef.close(data);
      }
    });
  }
}

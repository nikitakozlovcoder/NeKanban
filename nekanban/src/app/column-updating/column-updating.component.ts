import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
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
  name = new FormControl(this.data.column.name, [Validators.required, Validators.minLength(3)]);
  isLoaded = true;

  updateColumn() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isLoaded = false;
      this.columnService.updateColumn(this.data.column.id, this.name.value).subscribe({
        next: (data: Column[]) => {
          this.isLoaded = true;
          this.dialogRef.close(data);
        }
      });
    }
  }
}

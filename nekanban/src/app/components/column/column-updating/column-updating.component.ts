import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Column} from "../../../models/column";
import {FormControl, UntypedFormControl, Validators} from "@angular/forms";
import {ColumnService} from "../../../services/column.service";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-column-updating',
  templateUrl: './column-updating.component.html',
  styleUrls: ['./column-updating.component.css']
})
export class ColumnUpdatingComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {column: Column}, private columnService: ColumnService, public dialogRef: MatDialogRef<ColumnUpdatingComponent>) { }

  ngOnInit(): void {
  }
  name = new FormControl<string>(this.data.column.name, [Validators.required, Validators.minLength(3)]);
  isLoaded = new BehaviorSubject(true);

  updateColumn() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isLoaded.next(false);
      this.columnService.updateColumn(this.data.column.id, this.name.value!).subscribe({
        next: (data: Column[]) => {
          this.dialogRef.close(data);
        }
      }).add(() => {
        this.isLoaded.next(true);
      });
    }
  }
}

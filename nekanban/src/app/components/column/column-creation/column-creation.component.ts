import {Component, Inject, OnInit} from '@angular/core';
import {UntypedFormControl, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ColumnService} from "../../../services/column.service";
import {Column} from "../../../models/column";

@Component({
  selector: 'app-column-creation',
  templateUrl: './column-creation.component.html',
  styleUrls: ['./column-creation.component.css']
})
export class ColumnCreationComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number}, public dialogRef: MatDialogRef<ColumnCreationComponent>, private columnService: ColumnService) { }

  ngOnInit(): void {
  }
  name = new UntypedFormControl('', [Validators.required, Validators.minLength(3)]);
  isLoaded = true;

  createColumn() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isLoaded = false;
      this.columnService.addColumn(this.data.deskId, this.name.value).subscribe({
        next: (data: Column[]) => {
          this.isLoaded = true;
          this.dialogRef.close(data);
        }
      });
    }
  }

}

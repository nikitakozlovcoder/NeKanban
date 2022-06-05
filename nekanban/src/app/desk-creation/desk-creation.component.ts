import { Component, OnInit } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {DeskService} from "../services/desk.service";
import {Desk} from "../models/desk";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-desk-creation',
  templateUrl: './desk-creation.component.html',
  styleUrls: ['./desk-creation.component.css']
})
export class DeskCreationComponent implements OnInit {

  constructor(private deskService: DeskService, public dialogRef: MatDialogRef<DeskCreationComponent>) { }

  ngOnInit(): void {
  }
  name = new FormControl('', [Validators.required, Validators.minLength(6)]);
  isLoaded = true;

  createDesk() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isLoaded = false;
      this.deskService.addDesk(this.name.value).subscribe({
        next: (data: Desk) => {
          this.isLoaded = true;
          this.dialogRef.close(data);
        }
      });
    }
  }
}

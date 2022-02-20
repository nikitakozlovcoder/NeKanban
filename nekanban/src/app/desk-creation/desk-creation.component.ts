import { Component, OnInit } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {DeskService} from "../services/desk.service";
import {Router} from "@angular/router";
import {DeskComponent} from "../desk/desk.component";
import {Desk} from "../models/desk";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-desk-creation',
  templateUrl: './desk-creation.component.html',
  styleUrls: ['./desk-creation.component.css']
})
export class DeskCreationComponent implements OnInit {

  constructor(private deskService: DeskService, private router: Router, private deskComponent: DeskComponent, public dialogRef: MatDialogRef<DeskCreationComponent>) { }

  ngOnInit(): void {
  }
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);

  createDesk() {
    this.deskService.addDesk(this.name.value).subscribe({
      next: (data: Desk) => {
        this.dialogRef.close(data);
      }
    });
  }
}

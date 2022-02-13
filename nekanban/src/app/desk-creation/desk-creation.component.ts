import { Component, OnInit } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {DeskService} from "../services/desk.service";

@Component({
  selector: 'app-desk-creation',
  templateUrl: './desk-creation.component.html',
  styleUrls: ['./desk-creation.component.css']
})
export class DeskCreationComponent implements OnInit {

  constructor(private deskService: DeskService) { }

  ngOnInit(): void {
  }
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);

  createDesk() {
    this.deskService.addDesk(this.name.value);
  }
}

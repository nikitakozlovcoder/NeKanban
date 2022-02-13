import { Component, OnInit } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {DeskService} from "../services/desk.service";
import {Router} from "@angular/router";
import {DeskComponent} from "../desk/desk.component";

@Component({
  selector: 'app-desk-creation',
  templateUrl: './desk-creation.component.html',
  styleUrls: ['./desk-creation.component.css']
})
export class DeskCreationComponent implements OnInit {

  constructor(private deskService: DeskService, private router: Router, private deskComponent: DeskComponent) { }

  ngOnInit(): void {
  }
  name = new FormControl('', [Validators.required, Validators.minLength(8)]);

  createDesk() {
    this.deskService.addDesk(this.name.value);
    this.deskComponent.closeDialog();
    this.router.navigate(['']);
  }
}

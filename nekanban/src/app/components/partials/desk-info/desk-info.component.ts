import {Component, Input, OnInit} from '@angular/core';
import {Desk} from "../../../models/desk";
import {DeskUser} from "../../../models/deskUser";

@Component({
  selector: 'app-desk-info',
  templateUrl: './desk-info.component.html',
  styleUrls: ['./desk-info.component.css']
})
export class DeskInfoComponent implements OnInit {

  @Input() desk?: Desk;
  @Input() settingsLink?: any[];
  @Input() returnLink?: any[];
  constructor() { }

  ngOnInit(): void {
  }

  getDeskOwner() : DeskUser | undefined  {
    return this.desk?.deskUsers.find(el => el.isOwner);
  }

}

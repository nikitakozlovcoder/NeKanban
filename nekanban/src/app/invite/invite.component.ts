import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {DeskService} from "../services/desk.service";

@Component({
  selector: 'app-invite',
  templateUrl: './invite.component.html',
  styleUrls: ['./invite.component.css']
})
export class InviteComponent implements OnInit {

  constructor(private route: ActivatedRoute, private deskService: DeskService, private router: Router) { }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
          //console.log(params); // { orderby: "price" }
          this.deskService.inviteByLink(params["desk"]).subscribe({
            next: data => {
              this.router.navigate(['']);
            },
            error: err => {
              console.log(err);
              this.router.navigate(['']);
            }
          });
        }
      );
  }

}

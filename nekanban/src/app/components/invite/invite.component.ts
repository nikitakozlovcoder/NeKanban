import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {DialogService} from "../../services/dialog.service";
import {HttpErrorResponse} from "@angular/common/http";
import {DeskUserService} from "../../services/deskUser.service";

@Component({
  selector: 'app-invite',
  templateUrl: './invite.component.html',
  styleUrls: ['./invite.component.css']
})
export class InviteComponent implements OnInit {

  constructor(private route: ActivatedRoute, private deskUserService: DeskUserService, private router: Router,
              private readonly dialogService: DialogService) { }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
          this.deskUserService.inviteByLink(params["desk"]).subscribe({
            next: (result) => {
              this.router.navigate(['/desks', result.id]).then();
            },
            error: (err: HttpErrorResponse) => {
              this.dialogService.openToast("CantJoinOnLink");
              this.router.navigate(['']).then();
            }
          });
        }
      );
  }

}

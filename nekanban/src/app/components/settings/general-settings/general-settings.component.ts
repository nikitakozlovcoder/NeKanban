import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Desk} from "../../../models/desk";
import {RolesService} from "../../../services/roles.service";
import {Role} from "../../../models/Role";
import {DeskUserService} from "../../../services/deskUser.service";
import {FormControl, UntypedFormControl, Validators} from "@angular/forms";
import {DeskService} from "../../../services/desk.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {environment} from "../../../../environments/environment";
import {ConfirmationComponent} from "../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {MatDialog} from "@angular/material/dialog";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: ['./general-settings.component.css']
})
export class GeneralSettingsComponent implements OnInit {

  @Input() desk: Desk | undefined;
  @Output() deskChange = new EventEmitter<Desk>;
  @Input() roles: Role[] = [];
  @Input() desks: Desk[] = [];
  @Output() desksChange = new EventEmitter<Desk[]>;
  isRemoveDeskLoaded = new BehaviorSubject(true);
  isNameUpdateLoaded = new BehaviorSubject(true);
  isLinkLoaded = new BehaviorSubject(true);
  name = new FormControl<string>('', [Validators.required, Validators.minLength(6)]);

  constructor(public readonly rolesService: RolesService,
              private readonly deskService: DeskService,
              public readonly deskUserService: DeskUserService,
              public snackBar: MatSnackBar,
              private router: Router,
              public dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.name = new UntypedFormControl(this.desk!.name, [Validators.required, Validators.minLength(6)]);
  }

  updateDesk() {
    if (this.name.invalid) {
      this.name.markAsTouched();
    }
    else {
      this.isNameUpdateLoaded.next(false);
      this.deskService.updateDesk(this.desk!.id, this.name.value!).subscribe({
        next: (data: Desk) => {
          this.desk = data;
          this.deskChange.emit(this.desk);
          let index = this.desks.findIndex(el => el.id === this.desk!.id);
          this.desks[index].name = data.name;
          this.desksChange.emit(this.desks);
        }
      }).add(() => {
        this.isNameUpdateLoaded.next(true);
      });
    }
  }

  hasInviteLink() {
    return !(this.desk?.inviteLink === null);
  }

  getInviteLink() {
    if (this.hasInviteLink()) {
      return environment.angularBaseUrl + "invite?desk=" + this.desk?.inviteLink;
    }
    return null;
  }

  generateLink() {
    this.isLinkLoaded.next(false);
    this.deskService.setLink(this.desk!.id).subscribe( {
      next: data => {
        this.desk = data;
      },
      error: () => {
      }
    }).add(() => {
      this.isLinkLoaded.next(true);
    });
  }

  removeLink() {
    this.isLinkLoaded.next(false);
    this.deskService.removeLink(this.desk!.id).subscribe( {
      next: data => {
        this.desk = data;
      },
      error: () => {
      }
    }).add(() => {
      this.isLinkLoaded.next(true);
    });
  }

  removeDesk(deskId: number) {
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result == DialogActionTypes.Reject) {
        return;
      }
      this.makeDeskRemoval(deskId);
    });

  }

  private makeDeskRemoval(deskId: number) {
    this.isRemoveDeskLoaded.next(false);
    this.deskService.removeDesk(deskId).subscribe({
      next: () => {
        this.router.navigate(['']).then();
      },
      error: () => {
      }
    }).add(() => {
      this.isRemoveDeskLoaded.next(true);
    });
  }
}

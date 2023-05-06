import {Component, Inject, OnInit} from '@angular/core';
import {UntypedFormControl, Validators} from "@angular/forms";
import {RolesService} from "../../../../services/roles.service";
import {Role} from "../../../../models/Role";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-role-creation',
  templateUrl: './role-creation.component.html',
  styleUrls: ['./role-creation.component.css']
})
export class RoleCreationComponent implements OnInit {

  name = new UntypedFormControl('', [Validators.required, Validators.minLength(5)]);

  isLoaded = new BehaviorSubject(true);

  constructor(private readonly rolesService: RolesService,
              @Inject(MAT_DIALOG_DATA) public data: {deskId: number},
              public dialogRef: MatDialogRef<RoleCreationComponent>) { }

  ngOnInit(): void {
  }

  createRole() {
    if (this.name.invalid) {
      this.name.markAsTouched();
      return;
    }
    this.isLoaded.next(false);
    this.rolesService.createRole(this.data.deskId, this.name.value).subscribe({
      next: (data: Role[]) => {
        this.dialogRef.close(data);
      }
    }).add(() => {
      this.isLoaded.next(true);
    });
  }
}

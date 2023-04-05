import {Component, Inject, OnInit} from '@angular/core';
import {UntypedFormControl, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Role} from "../../../../models/Role";
import {RolesService} from "../../../../services/roles.service";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-role-updating',
  templateUrl: './role-updating.component.html',
  styleUrls: ['./role-updating.component.css']
})
export class RoleUpdatingComponent implements OnInit {

  name = new UntypedFormControl(this.data.role.name, [Validators.required, Validators.minLength(5)]);

  isLoaded = new BehaviorSubject(true);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {role: Role},
              private readonly rolesService: RolesService,
              public dialogRef: MatDialogRef<RoleUpdatingComponent>) { }

  ngOnInit(): void {
  }

  updateRole() {
    if (this.name.invalid) {
      this.name.markAsTouched();
      return;
    }
    this.isLoaded.next(false);
    this.rolesService.updateRole(this.data.role.id, this.name.value).subscribe({
      next: (data: Role[]) => {
        this.dialogRef.close(data);
      }
    }).add(() => {
      this.isLoaded.next(true);
    })
  }
}

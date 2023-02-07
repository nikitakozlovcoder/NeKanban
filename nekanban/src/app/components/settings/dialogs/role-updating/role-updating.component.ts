import {Component, Inject, OnInit} from '@angular/core';
import {UntypedFormControl, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Role} from "../../../../models/Role";
import {RolesService} from "../../../../services/roles.service";

@Component({
  selector: 'app-role-updating',
  templateUrl: './role-updating.component.html',
  styleUrls: ['./role-updating.component.css']
})
export class RoleUpdatingComponent implements OnInit {

  name = new UntypedFormControl(this.data.role.name, [Validators.required, Validators.minLength(5)]);

  isLoaded = true;

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
    this.isLoaded = false;
    this.rolesService.updateRole(this.data.role.id, this.name.value).subscribe({
      next: (data: Role[]) => {
        this.isLoaded = true;
        this.dialogRef.close(data);
      }
    })
  }
}

import {Component, Inject, OnInit} from '@angular/core';
import {UntypedFormControl, Validators} from "@angular/forms";
import {Todo} from "../../../../models/todo";
import {RolesService} from "../../../../services/roles.service";
import {Role} from "../../../../models/Role";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-role-creation',
  templateUrl: './role-creation.component.html',
  styleUrls: ['./role-creation.component.css']
})
export class RoleCreationComponent implements OnInit {

  name = new UntypedFormControl('', [Validators.required, Validators.minLength(5)]);

  isLoaded = true;

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
    this.isLoaded = false;
    this.rolesService.createRole(this.data.deskId, this.name.value).subscribe({
      next: (data: Role[]) => {
        this.isLoaded = true;
        this.dialogRef.close(data);
      }
    });
  }
}

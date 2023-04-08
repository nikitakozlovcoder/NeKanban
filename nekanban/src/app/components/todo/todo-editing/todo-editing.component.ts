import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {BehaviorSubject, last, map} from "rxjs";
import tinymce, {EditorOptions} from "tinymce";
import {EditorConfigService} from "../../../services/editor-config-service";
import {EditorUploaderService} from "../../../services/editor-uploader.service";

@Component({
  selector: 'app-todo-editing',
  templateUrl: './todo-editing.component.html',
  styleUrls: ['./todo-editing.component.css']
})
export class TodoEditingComponent implements OnInit {

  todoFormGroup = new FormGroup({
    id: new FormControl<number>(0),
    name: new FormControl<string>('', [Validators.required, Validators.minLength(3)]),
    body: new FormControl<string>('')
  })

  isLoaded = new BehaviorSubject(false);
  updateLoaded = new BehaviorSubject(true);
  editorLoaded = new BehaviorSubject(false);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number, toDo: Todo},
              private todoService: TodoService,
              public dialogRef: MatDialogRef<TodoEditingComponent>,
              private readonly editorUploaderService: EditorUploaderService) {
  }


  ngOnInit(): void {
    this.getToDo();
  }

  setLoaded() {
    this.editorLoaded.next(true);
  }

  imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.todoService.attachFile(this.todoFormGroup.controls.id.value!, formData).pipe(
      map(event => this.editorUploaderService.getEventMessage(event, progress)),
      last()
    ).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });

  getToDo() {
    this.isLoaded.next(false);
    this.todoService.getToDo(this.data.toDo.id).subscribe({
      next: (data) => {
        this.todoFormGroup.patchValue(data);
      }
    }).add(() => {
      this.isLoaded.next(true);
    })
  }

  updateToDo(): void {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.updateLoaded.next(false);
      tinymce.activeEditor?.uploadImages().then(() => {
        this.todoService.updateToDo(this.todoFormGroup.getRawValue() as Todo).subscribe({
          next: data => {
            this.dialogRef.close(data);
          },
          error: () => {
          }
        }).add(() => {
          this.updateLoaded.next(true);
        });
      });
    }
  }
}

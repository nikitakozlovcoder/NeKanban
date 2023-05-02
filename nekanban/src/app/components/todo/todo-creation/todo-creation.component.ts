import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {Todo} from "../../../models/todo";
import {
  BehaviorSubject,
  combineLatest,
  debounceTime, last,
  map,
  Subject,
  Subscription,
  switchMap
} from "rxjs";
import {EditorUploaderService} from "../../../services/editor-uploader.service";

@Component({
  selector: 'app-todo-creation',
  templateUrl: './todo-creation.component.html',
  styleUrls: ['./todo-creation.component.css']
})
export class TodoCreationComponent implements OnInit {

  draftSubject = new Subject();
  formLoaded = new BehaviorSubject(false);
  formSubmitLoaded = new BehaviorSubject(true);
  editorLoaded = new BehaviorSubject(false);
  firstUpdateRequest = true;
  private subscriptions = new Subscription();

  todoFormGroup = new FormGroup({
    id: new FormControl<number>(0),
    name: new FormControl<string>('', [Validators.required, Validators.minLength(3)]),
    body: new FormControl<string>('')
  })

  get isLoaded() {
    return combineLatest([this.formLoaded])
      .pipe(map(x => x.every(isLoaded => isLoaded)));
  }

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number},
              private todoService: TodoService,
              public dialogRef: MatDialogRef<TodoCreationComponent>,
              private readonly editorUploaderService: EditorUploaderService)
  {
  }


  ngOnInit(): void {

    this.getDraft();
    this.initDebounce();
    this.setFormListeners();
  }

  setLoaded() {
    this.editorLoaded.next(true);
  }

  applyDraft() {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.formSubmitLoaded.next(false);
      this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo).subscribe({
        next: (data: Todo) => {
          this.todoFormGroup.patchValue(data, {emitEvent : false});
          },
        complete: () => {
          this.todoService.applyDraft(this.todoFormGroup.controls.id.value!).subscribe({
            next: (data: Todo) => {
              this.formSubmitLoaded.next(true);
              this.dialogRef.close(
                <Todo>{
                  id: data.id,
                  name: data.name,
                  column: data.column,
                  toDoUsers: data.toDoUsers,
                  order: data.order,
                  code: data.code}
              );
            }
          });

        }
      }).add(() => {
        this.formSubmitLoaded.next(true);
      });
    }
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

  private getDraft() {
    this.formLoaded.next(false);
    this.todoService.getDraft(this.data.deskId).subscribe( {
      next: (data: Todo) => {
        this.todoFormGroup.patchValue(data, {emitEvent : false});
      }
    }).add(() => {
      this.formLoaded.next(true);
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1000),
      switchMap(() => this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo)))
      .subscribe({
        next: (data) => {
          this.todoFormGroup.controls.name.patchValue(data.name, {emitEvent: false});
          this.todoFormGroup.controls.id.patchValue(data.id, {emitEvent: false});
        },
    });
  }

  private setFormListeners() {
    this.subscriptions.add(this.todoFormGroup.controls.name.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    }));
    this.subscriptions.add(this.todoFormGroup.controls.body.valueChanges.subscribe({
      next: () => {
        if (this.firstUpdateRequest) {
          this.firstUpdateRequest = false;
          return;
        }
        this.draftSubject.next(1);
      }
    }));
  }
}

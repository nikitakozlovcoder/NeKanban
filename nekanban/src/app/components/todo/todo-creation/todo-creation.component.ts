import {Component, Inject, OnDestroy, OnInit} from '@angular/core';

import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {Todo} from "../../../models/todo";
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  map,
  Subject,
  Subscription,
  switchMap
} from "rxjs";

@Component({
  selector: 'app-todo-creation',
  templateUrl: './todo-creation.component.html',
  styleUrls: ['./todo-creation.component.css']
})
export class TodoCreationComponent implements OnInit, OnDestroy {

  draftSubject = new Subject();
  formLoaded = new BehaviorSubject(false);
  formSubmitLoaded = new BehaviorSubject(true);
  editorLoaded = new BehaviorSubject(false);
  firstUpdateRequest = true;
  private inputValueSubscription = new Subscription();
  private inputBodySubscription = new Subscription();

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
              public dialogRef: MatDialogRef<TodoCreationComponent>)
  {
  }


  ngOnInit(): void {

    this.getDraft();
    this.initDebounce();
    this.setFormListeners();
  }

  ngOnDestroy(): void {
    this.inputValueSubscription.unsubscribe();
    this.inputBodySubscription.unsubscribe();
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
                new Todo(data.id, data.name, data.column, data.toDoUsers, data.order, data.code)
              );
            }
          });

        }
      })

    }
  }

  imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.todoService.attachFile(this.todoFormGroup.controls.id.value!, formData).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });

  private getDraft() {
    this.formLoaded.next(false);
    this.todoService.getDraft(this.data.deskId).subscribe( {
      next: (data: Todo) => {
        this.formLoaded.next(true);
        this.todoFormGroup.patchValue(data, {emitEvent : false});
      }
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
    this.inputValueSubscription = this.todoFormGroup.controls.name.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    })
    this.inputBodySubscription = this.todoFormGroup.controls.body.valueChanges.subscribe({
      next: () => {
        if (this.firstUpdateRequest) {
          this.firstUpdateRequest = false;
          return;
        }
        this.draftSubject.next(1);
      }
    })
  }
}

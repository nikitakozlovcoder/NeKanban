import {Component, Inject, OnInit} from '@angular/core';

import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, UntypedFormControl, Validators} from "@angular/forms";
import {TodoService} from "../../../services/todo.service";
import {Todo} from "../../../models/todo";
import {BehaviorSubject, combineLatest, debounce, debounceTime, filter, interval, map, Subject, switchMap} from "rxjs";
import tinymce, {EditorOptions} from "tinymce";
import {UserStorageService} from "../../../services/userStorage.service";
import {environment} from "../../../../environments/environment";

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
  editorConfig: Partial<EditorOptions>;


  get isLoaded() {
    return combineLatest([this.formLoaded])
      .pipe(map(x => x.every(isLoaded => isLoaded)));
  }

  constructor(@Inject(MAT_DIALOG_DATA) public data: {deskId: number},
              private todoService: TodoService,
              public dialogRef: MatDialogRef<TodoCreationComponent>,
              private readonly userStorageService: UserStorageService)
  {
    this.editorConfig = {
      language: 'ru',
      language_url: `/assets/tinymce/ru/ru.js`,
      images_upload_handler: this.imageUploadHandler,
      automatic_uploads: false,
      base_url: '/tinymce',
      suffix: '.min',
      relative_urls: false,
      remove_script_host: false,
      document_base_url: environment.baseUrl,
      plugins: ['lists', 'link', 'image', 'table', 'code', 'help', 'wordcount']
    }
  }


  ngOnInit(): void {

    this.getDraft();
    this.initDebounce();
    this.setFormListeners();
  }

  todoFormGroup = new FormGroup({
    id: new FormControl<number>(0),
    name: new FormControl<string>('', [Validators.required, Validators.minLength(3)]),
    body: new FormControl<string>('')
  })

  setLoaded() {
    this.editorLoaded.next(true);
  }

  applyDraft() {
    if (this.todoFormGroup.invalid) {
      this.todoFormGroup.markAsTouched();
    }
    else {
      this.formSubmitLoaded.next(false);
      tinymce.activeEditor?.uploadImages().then( () => {
        this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo).subscribe({
          next: (data: Todo) => {
            this.todoFormGroup.patchValue(data, {emitEvent : false});
          },
          complete: () => {
            this.todoService.applyDraft(this.todoFormGroup.controls.id.value!).subscribe({
              next: (data: Todo) => {
                this.formSubmitLoaded.next(true);
                this.dialogRef.close(
                  new Todo(data.id, data.name, data.column, data.toDoUsers, data.order)
                );
              }
            });

          }
        })
      });

    }
  }

  updateDraft() {
    if (this.firstUpdateRequest) {
      this.firstUpdateRequest = false;
      return;
    }
    this.draftSubject.next(1);
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
      switchMap(() => this.todoService.updateDraft(this.todoFormGroup.getRawValue() as Todo))).subscribe({
        next: (data: Todo) => {
          this.todoFormGroup.controls.name.patchValue(data.name, {emitEvent: false});
          this.todoFormGroup.controls.id.patchValue(data.id, {emitEvent: false});
          //this.todoFormGroup.patchValue(data, {emitEvent : false});
        },
    });
  }

  private setFormListeners() {
    this.todoFormGroup.controls.name.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    })
  }
}

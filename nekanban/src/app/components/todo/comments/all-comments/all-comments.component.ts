import {Component, Inject, Input, OnInit} from '@angular/core';
import {Todo} from "../../../../models/todo";
import {FormControl, UntypedFormControl, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {Comment} from "../../../../models/comment";
import {BehaviorSubject, combineLatest, debounceTime, filter, map, Subject, switchMap} from "rxjs";
import tinymce, {EditorOptions} from "tinymce";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {Desk} from "../../../../models/desk";
import {DeskUser} from "../../../../models/deskUser";
import {Role} from "../../../../models/Role";
import {TodoService} from "../../../../services/todo.service";
import {RolesService} from "../../../../services/roles.service";
import {DataGeneratorService} from "../../../../services/dataGenerator.service";
import {CommentsService} from "../../../../services/comments.service";
import {DomSanitizer} from "@angular/platform-browser";
import {EditorConfigService} from "../../../../services/editor-config-service";
import {User} from "../../../../models/user";
import {MatSelect} from "@angular/material/select";
import {ConfirmationComponent} from "../../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../../constants/DialogActionTypes";
import {ViewStateTypes} from "../../../../constants/ViewStateTypes";

@Component({
  selector: 'app-all-comments',
  templateUrl: './all-comments.component.html',
  styleUrls: ['./all-comments.component.css']
})
export class AllCommentsComponent implements OnInit {

  readonly ViewStateTypes = ViewStateTypes;
  commentsUpdatingStates : ViewStateTypes[] = [];
  isSortDescending = true;
  comments: Comment[] = [];
  draftId?: number;
  commentInput = new FormControl<string>('', [this.commentLengthValidator()]);
  commentUpdatingFields: FormControl<string>[] = [];
  draftSubject = new Subject();
  commentsLoaded = new BehaviorSubject(false);
  commentsSendingLoaded = new BehaviorSubject(true);
  commentsUpdateLoaded : BehaviorSubject<boolean>[] = [];
  commentDraftLoaded = new BehaviorSubject(false);
  todoLoaded = new BehaviorSubject(false);
  notSend =  true;
  editorLoaded = new BehaviorSubject(false);
  editorConfig: Partial<EditorOptions>;
  private firstUpdateRequest = true;
  commentsUpdateEditorLoaded : BehaviorSubject<boolean>[] = [];

  @Input() todoId?: number;
  @Input() deskUser?: DeskUser;
  @Input() roles: Role[] = [];

  constructor(private toDoService: TodoService,
              public rolesService: RolesService,
              private dataGeneratorService: DataGeneratorService,
              private commentsService: CommentsService,
              public dialog: MatDialog,
              private readonly editorConfigService: EditorConfigService) {
    this.editorConfig = editorConfigService.getConfig(this.imageUploadHandler);
    this.editorConfig.max_height = 400;
  }

  get isLoaded() {
    return combineLatest([this.commentsLoaded, this.commentDraftLoaded, this.todoLoaded])
      .pipe(map(x => x.every(isLoaded => isLoaded)));
  }

  imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.commentsService.attachFile(this.draftId!, formData).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });

  ngOnInit(): void {
    this.getComments();
    this.getCommentDraft();
    this.initDebounce();
    this.setFormListeners();
  }

  setLoaded() {
    this.editorLoaded.next(true);
  }

  setUpdateEditorLoaded(index: number) {
    this.commentsUpdateEditorLoaded[index].next(true);
  }

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  sortComments(comments: Comment[]): Comment[] {
    if (this.isSortDescending) {
      return comments.sort(function (a: Comment, b: Comment) {
        if (a.createdAtUtc > b.createdAtUtc) {
          return -1;
        }
        if (a.createdAtUtc < b.createdAtUtc) {
          return 1;
        }
        return 0;
      });
    }
    return comments.sort(function (a: Comment, b: Comment) {
      if (a.createdAtUtc > b.createdAtUtc) {
        return 1;
      }
      if (a.createdAtUtc < b.createdAtUtc) {
        return -1;
      }
      return 0;
    });
  }

  toggleCommentsOrder() {
    this.isSortDescending = !this.isSortDescending;
    this.comments = this.sortComments(this.comments);
    this.RefreshUpdatingStatesAndFormControls();
  }

  getComments() {
    this.commentsLoaded.next(false);
    this.commentsService.getComments(this.todoId!).subscribe(
      {
        next: data => {
          this.commentsLoaded.next(true);
          this.comments = this.SortAndMapComments(data);
        },
        error: _ => {
        },
        complete: () => this.RefreshUpdatingStatesAndFormControls()
      }
    )

  }

  createComment() {
    this.commentInput.updateValueAndValidity();
    if (this.commentInput.invalid) {
      this.commentInput.markAsTouched();
    }
    else {
      this.commentsSendingLoaded.next(false);
      this.notSend = false;
      this.commentInput.clearValidators();
      this.commentsService.updateDraft(this.draftId!, this.commentInput.value!).subscribe({
        next: (data) => {
          //this.commentInput.setValue(data.body, {emitEvent : false});
          this.draftId = data.id;
        },
        complete: () => {
          this.commentsService.applyDraft(this.draftId!).subscribe({
            next: data => {
              this.commentsSendingLoaded.next(true);
              this.comments = this.SortAndMapComments(data);
              this.commentInput.setValue("", {emitEvent: false});
              this.getCommentDraft();
            },
            error: _ => {
            },
            complete: () => this.RefreshUpdatingStatesAndFormControls()
          });
        }
      });
    }
  }

  updateComment(id: number, index: number) {
    if (this.commentUpdatingFields[index].invalid) {
      this.commentUpdatingFields[index].markAsTouched();
    }
    else {
      this.commentsUpdateLoaded[index].next(false);
      this.commentsService.updateComment(id, this.commentUpdatingFields[index].value).subscribe({
        next: data => {
          this.commentsUpdateLoaded[index].next(true);
          this.comments = this.SortAndMapComments(data);
          this.commentsUpdatingStates[index] = ViewStateTypes.Show;
          this.commentsUpdateEditorLoaded[index].next(false);
        },
        error: _ => {
        }
      })
    }
  }

  deleteComment(id: number) {
    const dialogRef = this.dialog.open(ConfirmationComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result == DialogActionTypes.Reject) {
        return;
      }
      this.makeDeletion(id);
    });

  }

  updateCommentDraft() {
    console.log(this.firstUpdateRequest)
    console.log('not send', this.notSend)
    if (this.firstUpdateRequest) {
      this.firstUpdateRequest = false;
      return;
    }
    this.draftSubject.next(1);
  }

  commentLengthValidator() : ValidatorFn {
    return (): ValidationErrors | null => {
      console.log(2)
      if (tinymce.get('comment-tinymce')?.initialized) {
        console.log(tinymce.get('comment-tinymce')!.getContent({format : 'text'}))
        return tinymce.get('comment-tinymce')!.getContent({format : 'text'}).length < 10 ? {commentMinLength: true} : null;
      }
      return null;
    };
  }


  handleCommentChange(comment: Comment) {
  }

  handleCommentDeletion(id: number) {
    this.comments.splice(this.comments.findIndex(el => el.id === id), 1);
  }

  private makeDeletion(id: number) {
    this.commentsLoaded.next(false);
    if (id == this.deskUser!.user.id) {
      this.commentsService.deleteOwnComment(id).subscribe({
        next: data => {
          this.commentsLoaded.next(true);
          this.comments = this.SortAndMapComments(data);
        },
        error: _ => {
        },
        complete: () => this.RefreshUpdatingStatesAndFormControls()
      })
    }
    else if (this.checkUserPermission(this.deskUser!, 'DeleteAnyComments')) {
      this.commentsService.deleteComment(id).subscribe({
        next: data => {
          this.commentsLoaded.next(true);
          this.comments = this.SortAndMapComments(data);
        },
        error: _ => {
        },
        complete: () => this.RefreshUpdatingStatesAndFormControls()
      })
    }
  }

  hideUpdatingField(index: number) {
    this.commentsUpdatingStates[index] = ViewStateTypes.Show;
    this.commentUpdatingFields[index] = new UntypedFormControl(this.comments[index].body, [Validators.required, Validators.minLength(10)]);
    this.commentsUpdateEditorLoaded[index].next(false);
  }

  private SortAndMapComments(comments: Comment[]) {
    return this.sortComments(comments.map(el => {
      return new Comment(el.id, el.body, el.deskUser, new Date(el.createdAtUtc));
    }));
  }

  private RefreshUpdatingStatesAndFormControls() {
    this.commentsUpdatingStates = [];
    this.commentsUpdateLoaded = [];
    this.comments.forEach(() => {
      this.commentsUpdatingStates.push(ViewStateTypes.Show);
      this.commentsUpdateLoaded.push(new BehaviorSubject<boolean>(true));
      this.commentsUpdateEditorLoaded.push(new BehaviorSubject<boolean>(false));
    })
    this.commentUpdatingFields = [];
    this.comments.forEach(el => {
      this.commentUpdatingFields.push(new UntypedFormControl(el.body, [Validators.required, Validators.minLength(10)]));
    });
  }

  showCommentUpdateForm(index: number) {
    this.commentsUpdatingStates[index] = ViewStateTypes.Update;
  }

  private getCommentDraft() {
    this.commentDraftLoaded.next(false);
    this.commentsService.getDraft(this.todoId!).subscribe({
      next: (data) => {
        this.commentDraftLoaded.next(true);
        this.notSend = true;
        this.commentInput.setValue(data.body, {emitEvent: false});
        this.draftId = data.id;
      }
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1000),
      filter(() => this.notSend),
      switchMap(() => this.commentsService.updateDraft(this.draftId!, this.commentInput.value!))).subscribe({
      next: (data) => {
        console.log("Retrieved value");
        //this.commentInput.setValue(data.body, {emitEvent : false});
        this.draftId = data.id;
      }
    });
  }

  private setFormListeners() {
    this.commentInput.valueChanges.subscribe({
      next: () => {
        this.commentInput.addValidators(this.commentLengthValidator());
      }
    })
  }

}

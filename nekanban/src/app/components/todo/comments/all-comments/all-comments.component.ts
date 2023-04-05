import {Component, Input, OnInit} from '@angular/core';
import {FormControl, ValidationErrors, ValidatorFn} from "@angular/forms";
import {Comment} from "../../../../models/comment";
import {BehaviorSubject, debounceTime, filter, last, map, from, Subject, Subscription, switchMap} from "rxjs";
import tinymce, {EditorOptions} from "tinymce";
import {MatDialog} from "@angular/material/dialog";
import {DeskUser} from "../../../../models/deskUser";
import {Role} from "../../../../models/Role";
import {TodoService} from "../../../../services/todo.service";
import {RolesService} from "../../../../services/roles.service";
import {DataGeneratorService} from "../../../../services/dataGenerator.service";
import {CommentsService} from "../../../../services/comments.service";
import {EditorConfigService} from "../../../../services/editor-config-service";
import {ViewStateTypes} from "../../../../constants/ViewStateTypes";
import {ValidationService} from "../../../../services/validation.service";
import {EditorUploaderService} from "../../../../services/editor-uploader.service";
import {UntilDestroy, untilDestroyed} from "@ngneat/until-destroy";
import {Todo} from "../../../../models/todo";
import {Column} from "../../../../models/column";

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'app-all-comments',
  templateUrl: './all-comments.component.html',
  styleUrls: ['./all-comments.component.css']
})
export class AllCommentsComponent implements OnInit {

  isSortDescending = true;
  comments: Comment[] = [];
  draftId?: number;
  commentInput = new FormControl<string>('', []);
  draftSubject = new Subject();
  commentsLoaded = new BehaviorSubject(false);
  commentsSendingLoaded = new BehaviorSubject(true);
  commentDraftLoaded = new BehaviorSubject(false);
  notSend =  true;
  editorLoaded = new BehaviorSubject(false);
  private firstUpdateRequest = true;
  toggleComments = new Subject<any>();

  @Input() todoId?: number;
  @Input() deskUser?: DeskUser;
  @Input() roles: Role[] = [];

  commentInputSubscription = new Subscription();

  constructor(private toDoService: TodoService,
              public rolesService: RolesService,
              private dataGeneratorService: DataGeneratorService,
              private commentsService: CommentsService,
              public dialog: MatDialog,
              private readonly editorUploaderService: EditorUploaderService,
              private readonly validationService: ValidationService) {
  }

  imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.commentsService.attachFile(this.draftId!, formData).pipe(
      map(event => this.editorUploaderService.getEventMessage(event, progress)),
      last()
    ).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });

  ngOnInit(): void {
    this.getComments();
    if (this.rolesService.userHasPermission(this.roles, this.deskUser!, this.rolesService.permissionsTypes.AddOrUpdateOwnComments)) {
      this.getCommentDraft();
      this.initDebounce();
      this.setFormListeners();
      this.commentInput.markAsUntouched();
    }
  }

  setLoaded() {
    this.editorLoaded.next(true);
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
    this.toggleComments.next(1);
    this.comments = this.sortComments(this.comments);
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
        }
      });
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
            }
          });
        }
      }).add(() => {
        this.commentsSendingLoaded.next(true);
      });
    }
  }

  commentLengthValidator() : ValidatorFn {
    return this.validationService.editorMinLengthValidator(tinymce.get('comment-tinymce' + this.todoId!.toString()), 10);
  }

  handleCommentDeletion(id: number) {
    this.comments.splice(this.comments.findIndex(el => el.id === id), 1);
  }

  private SortAndMapComments(comments: Comment[]) {
    return this.sortComments(comments.map(el => {
      return <Comment>{
        id: el.id,
        body: el.body,
        deskUser: el.deskUser,
        createdAtUtc: new Date(el.createdAtUtc)};
    }));
  }

  private getCommentDraft() {
    this.commentDraftLoaded.next(false);
    this.commentsService.getDraft(this.todoId!).subscribe({
      next: (data) => {
        this.commentDraftLoaded.next(true);
        this.notSend = true;
        this.commentInput.setValue(data.body, {emitEvent: false});
        this.commentInput.addValidators(this.commentLengthValidator());
        this.draftId = data.id;
      }
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1000),
      filter(() => this.notSend),
      switchMap(() => this.commentsService.updateDraft(this.draftId!, this.commentInput.value!))).subscribe({
      next: (data) => {
        this.draftId = data.id;
      }
    });
  }

  private setFormListeners() {
    this.commentInputSubscription = this.commentInput.valueChanges.subscribe({
      next: () => {
        this.commentInput.addValidators(this.commentLengthValidator());
        if (this.firstUpdateRequest) {
          this.firstUpdateRequest = false;
          return;
        }
        this.draftSubject.next(1);
      }
    })
  }

}

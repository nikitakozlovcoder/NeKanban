import {Component, Input, OnInit} from '@angular/core';
import {FormControl, ValidationErrors, ValidatorFn} from "@angular/forms";
import {Comment} from "../../../../models/comment";
import {BehaviorSubject, debounceTime, filter, Subject, switchMap} from "rxjs";
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
  editorConfig: Partial<EditorOptions>;
  private firstUpdateRequest = true;
  toggleComments = new Subject<any>();

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
    this.commentInput.markAsUntouched();
  }

  setLoaded() {
    this.editorLoaded.next(true);
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
      });
    }
  }

  updateCommentDraft() {
    if (this.firstUpdateRequest) {
      this.firstUpdateRequest = false;
      return;
    }
    this.draftSubject.next(1);
  }

  commentLengthValidator() : ValidatorFn {
    return (): ValidationErrors | null => {
      if (tinymce.get('comment-tinymce')?.initialized) {
        return tinymce.get('comment-tinymce')!.getContent({format : 'text'}).length < 10 ? {commentMinLength: true} : null;
      }
      return null;
    };
  }

  handleCommentDeletion(id: number) {
    this.comments.splice(this.comments.findIndex(el => el.id === id), 1);
  }

  private SortAndMapComments(comments: Comment[]) {
    return this.sortComments(comments.map(el => {
      return new Comment(el.id, el.body, el.deskUser, new Date(el.createdAtUtc));
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
    this.commentInput.valueChanges.subscribe({
      next: () => {
        this.commentInput.addValidators(this.commentLengthValidator());
      }
    })
  }

}

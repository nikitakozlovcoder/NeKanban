import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Comment} from "../../../../models/comment";
import {ViewStateTypes} from "../../../../constants/ViewStateTypes";
import {BehaviorSubject} from "rxjs";
import {FormControl, ValidationErrors, ValidatorFn} from "@angular/forms";
import tinymce, {EditorOptions} from "tinymce";
import {EditorConfigService} from "../../../../services/editor-config-service";
import {CommentsService} from "../../../../services/comments.service";
import {DeskUser} from "../../../../models/deskUser";
import {RolesService} from "../../../../services/roles.service";
import {Role} from "../../../../models/Role";
import {ConfirmationComponent} from "../../../dialogs/confirmation/confirmation.component";
import {DialogActionTypes} from "../../../../constants/DialogActionTypes";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-single-comment',
  templateUrl: './single-comment.component.html',
  styleUrls: ['./single-comment.component.css']
})
export class SingleCommentComponent implements OnInit {

  readonly ViewStateTypes = ViewStateTypes;

  @Input() comment?: Comment;
  @Output() commentChange = new EventEmitter<Comment>;

  @Input() roles: Role[] = [];
  @Input() deskUser?: DeskUser;

  @Output() commentsDeletion = new EventEmitter<number>;

  commentUpdatingState : ViewStateTypes = ViewStateTypes.Show;
  commentUpdateEditorLoaded = new BehaviorSubject(false);
  commentUpdateLoaded = new BehaviorSubject(true);
  commentDeleteLoaded = new BehaviorSubject(true);
  commentUpdatingField = new FormControl<string>(this.comment ? this.comment!.body : '');
  editorConfig: Partial<EditorOptions>;

  constructor(private readonly editorConfigService: EditorConfigService,
              private readonly commentsService: CommentsService,
              private readonly rolesService: RolesService,
              public dialog: MatDialog) {
    this.editorConfig = editorConfigService.getConfig(this.imageUploadHandler);
    this.editorConfig.max_height = 400;
  }

  ngOnInit(): void {
    this.commentUpdatingField = new FormControl<string>(this.comment ? this.comment!.body : '', [this.commentLengthValidator()]);
    this.setFormListeners();
  }

  imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.commentsService.attachFile(this.comment!.id, formData).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.roles, deskUser, permissionName);
  }

  setUpdateEditorLoaded() {
    this.commentUpdateEditorLoaded.next(true);
  }

  showCommentUpdateForm() {
    this.commentUpdatingState = ViewStateTypes.Update;
    this.commentUpdatingField = new FormControl<string>(this.comment!.body, [this.commentLengthValidator()]);
  }

  hideUpdatingField() {
    this.commentUpdatingState = ViewStateTypes.Show;
    this.commentUpdatingField = new FormControl<string>(this.comment!.body, [this.commentLengthValidator()]);
    this.commentUpdateEditorLoaded.next(false);
  }

  updateComment() {
    this.commentUpdatingField.updateValueAndValidity();
    if (this.commentUpdatingField.invalid) {
      this.commentUpdatingField.markAsTouched();
    }
    else {
      this.commentUpdateLoaded.next(false);
      this.commentUpdatingField.clearValidators();
      this.commentsService.updateComment(this.comment!.id, this.commentUpdatingField.value!).subscribe({
        next: data => {
          this.commentUpdateLoaded.next(true);
          this.comment = data.find(el => el.id === this.comment?.id);
          this.comment!.createdAtUtc = new Date(this.comment!.createdAtUtc);
          this.commentChange.emit(this.comment);
          this.commentUpdatingState = ViewStateTypes.Show;
          this.commentUpdateEditorLoaded.next(false);
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

  private makeDeletion(id: number) {
    this.commentDeleteLoaded.next(false);
    if (id == this.deskUser!.user.id) {
      this.commentsService.deleteOwnComment(id).subscribe({
        next: data => {
          this.commentDeleteLoaded.next(true);
          this.comment = data.find(el => el.id === this.comment?.id);
          this.commentsDeletion.emit(id);
        },
        error: _ => {
        }
      })
    }
    else if (this.checkUserPermission(this.deskUser!, 'DeleteAnyComments')) {
      this.commentsService.deleteComment(id).subscribe({
        next: data => {
          this.commentDeleteLoaded.next(true);
          this.comment = data.find(el => el.id === this.comment?.id);
          this.commentsDeletion.emit(id);
        },
        error: _ => {
        }
      })
    }
  }

  private setFormListeners() {
    this.commentUpdatingField.valueChanges.subscribe({
      next: () => {
        this.commentUpdatingField.addValidators(this.commentLengthValidator());
      }
    })
  }

  private commentLengthValidator() : ValidatorFn {
    return (): ValidationErrors | null => {
      if (tinymce.get(`comment-tinymce-update${this.comment!.id}`)?.initialized) {
        return tinymce.get(`comment-tinymce-update${this.comment!.id}`)!.getContent({format : 'text'}).length < 10 ? {commentMinLength: true} : null;
      }
      return null;
    };
  }
}

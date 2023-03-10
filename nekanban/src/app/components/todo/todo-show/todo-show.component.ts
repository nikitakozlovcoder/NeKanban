import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {Todo} from "../../../models/todo";
import {FormControl, UntypedFormControl, Validators} from "@angular/forms";
import {Desk} from "../../../models/desk";
import {User} from "../../../models/user";
import {Comment} from "../../../models/comment";
import {MatSelect} from "@angular/material/select";
import {TodoService} from "../../../services/todo.service";
import {RolesService} from "../../../services/roles.service";
import {DeskUser} from "../../../models/deskUser";
import {DataGeneratorService} from "../../../services/dataGenerator.service";
import {CommentsService} from "../../../services/comments.service";
import LoadingStateTypes from "../../../constants/LoadingStateTypes";
import {ViewStateTypes} from "../../../constants/ViewStateTypes";
import {DialogActionTypes} from "../../../constants/DialogActionTypes";
import {Role} from "../../../models/Role";
import {ConfirmationComponent} from "../../dialogs/confirmation/confirmation.component";
import {BehaviorSubject, debounceTime, Subject, switchMap} from "rxjs";

@Component({
  selector: 'app-task-creation',
  templateUrl: './todo-show.component.html',
  styleUrls: ['./todo-show.component.css']
})
export class TodoShowComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {todoId: number, isEdit: boolean, desk: Desk, deskUser: DeskUser, roles: Role[]}, private toDoService: TodoService, public rolesService: RolesService, public dialogRef: MatDialogRef<TodoShowComponent>, private dataGeneratorService: DataGeneratorService,
              private commentsService: CommentsService, public dialog: MatDialog) {
    this.dialogRef.beforeClosed().subscribe(() => this.closeDialog());
  }

  readonly LoadingStateTypes = LoadingStateTypes;

  readonly ViewStateTypes = ViewStateTypes;

  todo: Todo | undefined;

  usersSelected : number[] = [];

  userSelected : number[] = [];

  users = new UntypedFormControl(this.usersSelected);

  user = new UntypedFormControl(this.userSelected);

  isLoaded = true;

  commentsUpdatingStates : ViewStateTypes[] = [];

  isSortDescending = true;

  comments: Comment[] = [];

  draftId: number | undefined;

  commentInput = new FormControl<string>('', [Validators.required, Validators.minLength(10)]);

  commentUpdatingFields: UntypedFormControl[] = [];

  draftSubject = new Subject();

  commentsLoaded = new BehaviorSubject(false);

  commentsSendingLoaded = new BehaviorSubject(true);

  commentsUpdateLoaded : BehaviorSubject<boolean>[] = [];

  ngOnInit(): void {
    this.getComments();
    this.getCommentDraft();
    this.toDoService.getToDo(this.data.todoId).subscribe({
      next: value => {
        this.todo = value;
        this.usersSelected = this.getIdsOfSelectedUsers();
        this.userSelected = this.getIdOfSingleUser();
      }
    })
    this.initDebounce();
    this.setFormListeners();
  }

  closeDialog() {
    this.dialogRef.close(this.todo!);
  }
  getToDoCreator() {
    return this.todo!.toDoUsers.find(el => el.toDoUserType == 0);
  }

  getIdOfSingleUser() {
    let ids : number[] = [];
    let founded = this.getToDoUsers().find(el => el.deskUser.user.id === this.data.deskUser.user.id);
    if (founded !== undefined) {
      ids.push(founded.deskUser.user.id);
    }
    return ids;
  }

  getToDoUsers() {
    return this.todo!.toDoUsers.filter(el => el.toDoUserType != 0);
  }

  getDeskUsers() : User[] {
    let deskUsers: User[] = [];
    this.data.desk.deskUsers.forEach( el => {
      deskUsers.push(el.user);
    })
    return deskUsers;
  }
  getAllTodoUsers() : User[] {
    let todoUsers : User[] = [];
    this.todo!.toDoUsers.forEach( el => {
      todoUsers.push(el.deskUser.user);
    })
    return todoUsers;
  }
  getIdsOfSelectedUsers() : number[] {
    let selectedUsers : User[] = this.getDeskUsers().filter(el => this.getAllTodoUsers().some(someEl => someEl.id === el.id) && this.todo!.toDoUsers.find(todoUser => todoUser.deskUser.user.id === el.id  && todoUser.toDoUserType != 0));
    let ids : number[] = [];
    selectedUsers.forEach( el => {
      ids.push(el.id);
    })
    return ids;
  }
  changeUsers(select:MatSelect)  {
    let newIds : number[] = select.value;
    let appearedIds: number[] = [];
    newIds.forEach(el => {
      if (!this.usersSelected.includes(el)) {
        appearedIds.push(el);
      }
    })
    let disappearedIds : number[] = [];
    this.usersSelected.forEach( el => {
      if (!newIds.includes(el)) {
        disappearedIds.push(el);
      }
    })

    appearedIds.forEach(el => {
      this.isLoaded = false;
      this.dialogRef.disableClose = true;
      let deskUser = this.data.desk.deskUsers.find(obj => obj.user.id === el);
      this.toDoService.assignUser(this.data.todoId, deskUser!.id).subscribe({
        next: data => {
          if (appearedIds.indexOf(el) === appearedIds.length - 1) {
            this.isLoaded = true;
            this.dialogRef.disableClose = false;
          }
          this.todo!.toDoUsers = data.toDoUsers;
        },
        error: _ => {
        }
      })
    })
    disappearedIds.forEach( el => {
      this.isLoaded = false;
      this.dialogRef.disableClose = true;
      let todo = this.todo!.toDoUsers.find(obj => obj.deskUser.user.id === el && obj.toDoUserType != 0);
      this.toDoService.removeUser(todo!.id).subscribe({
        next: data => {
          if (disappearedIds.indexOf(el) === disappearedIds.length - 1) {
            this.isLoaded = true;
            this.dialogRef.disableClose = false;
          }
          this.todo!.toDoUsers = data.toDoUsers;
        },
        error: _ => {
        }
      })
    })
    this.usersSelected  = newIds;
  }

  checkUserPermission(deskUser: DeskUser, permissionName: string) {
    return this.rolesService.userHasPermission(this.data.roles, deskUser, permissionName);
  }

  changeSingleUser(select:MatSelect) {
    let newIds : number[] = select.value;
    console.log(select.value);
    if (newIds.length === 0) {
      let todo = this.todo!.toDoUsers.find(obj => obj.deskUser.user.id === this.data.deskUser.user.id);
      if (todo !== undefined) {
        this.isLoaded = false;
        this.dialogRef.disableClose = true;
        this.toDoService.removeUser(todo.id).subscribe({
          next: data => {
            this.isLoaded = true;
            this.todo!.toDoUsers = data.toDoUsers;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }

    }
    else {
      if (!this.usersSelected.includes(this.data.deskUser.user.id)) {
        this.isLoaded = false;
        this.dialogRef.disableClose = true;
        this.toDoService.assignUser(this.data.todoId, this.data.deskUser.id).subscribe({
          next: data => {
            this.isLoaded = true;
            this.todo!.toDoUsers = data.toDoUsers;
            this.dialogRef.disableClose = false;
          },
          error: _ => {
          }
        })
      }
    }
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
    this.commentsService.getComments(this.data.todoId).subscribe(
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
    if (this.commentInput.invalid) {
      this.commentInput.markAsTouched();
    }
    else {
      this.commentsSendingLoaded.next(false);
      this.commentsService.applyDraft(this.draftId!).subscribe({
        next: data => {
          this.commentsSendingLoaded.next(true);
          this.comments = this.SortAndMapComments(data);
          this.commentInput.setValue("", {emitEvent: false})
          this.getCommentDraft();
          this.commentInput.markAsUntouched();
        },
        error: _ => {
        },
        complete: () => this.RefreshUpdatingStatesAndFormControls()
      })
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
    this.commentsLoaded.next(false);
    if (id == this.data.deskUser.user.id) {
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
    else if (this.checkUserPermission(this.data.deskUser, 'DeleteAnyComments')) {
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
    this.commentsService.getDraft(this.data.todoId).subscribe({
      next: (data) => {
        this.commentInput.setValue(data.body, {emitEvent: false});
        this.draftId = data.id;
      }
    });
  }

  private initDebounce() {
    this.draftSubject.pipe(debounceTime(1000),
      switchMap(() => this.commentsService.updateDraft(this.draftId!, this.commentInput.value!))).subscribe({
      next: (data: Comment) => {
        this.commentInput.setValue(data.body, {emitEvent : false});
      },
    });
  }

  private setFormListeners() {
    this.commentInput.valueChanges.subscribe({
      next: () => {
        this.draftSubject.next(1);
      }
    })
  }
}

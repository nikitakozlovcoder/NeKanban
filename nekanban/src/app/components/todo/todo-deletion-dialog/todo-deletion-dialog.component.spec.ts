import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoDeletionDialogComponent } from './todo-deletion-dialog.component';

describe('TodoDeletionDialogComponent', () => {
  let component: TodoDeletionDialogComponent;
  let fixture: ComponentFixture<TodoDeletionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoDeletionDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoDeletionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoEditingComponent } from './todo-editing.component';

describe('TodoEditingComponent', () => {
  let component: TodoEditingComponent;
  let fixture: ComponentFixture<TodoEditingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoEditingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TodoEditingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

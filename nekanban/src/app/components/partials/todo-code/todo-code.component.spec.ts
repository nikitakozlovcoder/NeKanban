import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoCodeComponent } from './todo-code.component';

describe('TodoCodeComponent', () => {
  let component: TodoCodeComponent;
  let fixture: ComponentFixture<TodoCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoCodeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

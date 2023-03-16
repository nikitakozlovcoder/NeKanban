import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleCommentComponent } from './single-comment.component';

describe('SingleCommentComponent', () => {
  let component: SingleCommentComponent;
  let fixture: ComponentFixture<SingleCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SingleCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TinymceEditorComponent } from './tinymce-editor.component';

describe('TinymceEditorComponent', () => {
  let component: TinymceEditorComponent;
  let fixture: ComponentFixture<TinymceEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TinymceEditorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TinymceEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

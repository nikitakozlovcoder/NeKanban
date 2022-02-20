import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeskCreationDialogComponent } from './desk-creation-dialog.component';

describe('DeskCreationDialogComponent', () => {
  let component: DeskCreationDialogComponent;
  let fixture: ComponentFixture<DeskCreationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeskCreationDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeskCreationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

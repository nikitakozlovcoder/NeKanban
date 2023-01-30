import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnCreationComponent } from './column-creation.component';

describe('ColumnCreationComponent', () => {
  let component: ColumnCreationComponent;
  let fixture: ComponentFixture<ColumnCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ColumnCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnUpdatingComponent } from './column-updating.component';

describe('ColumnUpdatingComponent', () => {
  let component: ColumnUpdatingComponent;
  let fixture: ComponentFixture<ColumnUpdatingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ColumnUpdatingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnUpdatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

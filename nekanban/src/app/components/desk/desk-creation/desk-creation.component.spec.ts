import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeskCreationComponent } from './desk-creation.component';

describe('DeskCreationComponent', () => {
  let component: DeskCreationComponent;
  let fixture: ComponentFixture<DeskCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeskCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeskCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

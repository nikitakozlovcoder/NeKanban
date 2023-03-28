import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeskInfoComponent } from './desk-info.component';

describe('DeskInfoComponent', () => {
  let component: DeskInfoComponent;
  let fixture: ComponentFixture<DeskInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeskInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeskInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleUpdatingComponent } from './role-updating.component';

describe('RoleUpdatingComponent', () => {
  let component: RoleUpdatingComponent;
  let fixture: ComponentFixture<RoleUpdatingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoleUpdatingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleUpdatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

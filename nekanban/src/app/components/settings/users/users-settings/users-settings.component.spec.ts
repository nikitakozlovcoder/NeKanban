import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersSettingsComponent } from './users-settings.component';

describe('UsersSettingsComponent', () => {
  let component: UsersSettingsComponent;
  let fixture: ComponentFixture<UsersSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

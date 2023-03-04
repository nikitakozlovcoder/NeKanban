import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesSettingsComponent } from './roles-settings.component';

describe('RolesSettingsComponent', () => {
  let component: RolesSettingsComponent;
  let fixture: ComponentFixture<RolesSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesSettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolesSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

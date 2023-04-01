import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RawHtmlViewerComponent } from './raw-html-viewer.component';

describe('RawHtmlViewerComponent', () => {
  let component: RawHtmlViewerComponent;
  let fixture: ComponentFixture<RawHtmlViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RawHtmlViewerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RawHtmlViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

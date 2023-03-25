import {AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-raw-html-viewer',
  templateUrl: './raw-html-viewer.component.html',
  styleUrls: ['./raw-html-viewer.component.css']
})
export class RawHtmlViewerComponent implements AfterViewInit {

  @Input() html?: string;
  @ViewChild('description') description?: ElementRef;
  constructor() { }

  ngAfterViewInit(): void {
    const images = this.description?.nativeElement.querySelectorAll('img');
    images.forEach((el: HTMLImageElement) => {
      const src = el.src;
      el.src = environment.placeholderImage;
      const img = new Image();
      img.src = src;
      img.onload = () => {
        el.src = img.src;
      }
    });
  }
}

import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormControl} from "@angular/forms";
import {EditorConfigService} from "../../../services/editor-config-service";
import tinymce, {EditorOptions} from "tinymce";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-tinymce-editor',
  templateUrl: './tinymce-editor.component.html',
  styleUrls: ['./tinymce-editor.component.css']
})
export class TinymceEditorComponent implements OnInit {

  @Input() editorFormControl = new FormControl<string>('');
  @Output() editorFormControlChange = new EventEmitter<FormControl<string|null>>();
  @Output() editorOnInit = new EventEmitter();
  @Input() imageUploadHandler?: (blobInfo: any, progress: any) => Promise<string>;
  @Input() editorId?: string;
  editorConfig?: Partial<EditorOptions>;

  constructor(private readonly editorConfigService: EditorConfigService) {

  }

  ngOnInit(): void {
    this.editorConfig = this.editorConfigService.getConfig(this.imageUploadHandler);
    this.editorConfig.max_height = 500;
    this.editorConfig.init_instance_callback = (editor) => {
      editor.on('input', ()=> {
        this.editorChange();
      })
    }
  }

  editorInit() {
    const images = tinymce.get(this.editorId!.toString())?.contentDocument.querySelectorAll('img');
    if (images) {
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
    this.editorOnInit.emit();
  }

  editorChange() {
    this.editorFormControlChange.emit(this.editorFormControl);
  }

}

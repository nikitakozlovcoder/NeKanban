import {EditorOptions} from "tinymce";
import {environment} from "../../environments/environment";
import {Injectable} from "@angular/core";

@Injectable()
export class EditorConfigService {

  public constructor() {
  }

  getConfig(imageUploadHandler?: (blobInfo: any, progress: any) => Promise<string>) : Partial<EditorOptions> {
    return {
      language: 'ru',
      language_url: `/assets/tinymce/ru/ru.js`,
      images_upload_handler: imageUploadHandler,
      automatic_uploads: true,
      base_url: '/tinymce',
      suffix: '.min',
      relative_urls: false,
      remove_script_host: false,
      document_base_url: environment.baseUrl,
      branding: false,
      promotion: false,
      content_style: "body {font-size: 14px; font-family: Roboto, \"Helvetica Neue\", sans-serif;}",
      plugins: ['lists', 'link', 'image', 'table', 'code', 'help', 'wordcount', 'autoresize'],
    };
  }

  /*private imageUploadHandler = (blobInfo: any, progress: any) => new Promise<string>((resolve, reject) => {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    this.todoService.attachFile(this.todoId!, formData).subscribe({
      next: (data) => {
        resolve(data);
      }
    });
  });*/
}

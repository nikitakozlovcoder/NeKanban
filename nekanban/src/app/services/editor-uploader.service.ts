import {HttpEvent, HttpEventType} from "@angular/common/http";
import {Injectable} from "@angular/core";

@Injectable()
export class EditorUploaderService {
  getEventMessage(event: HttpEvent<any>, progress: any) {
    switch (event.type) {
      case HttpEventType.UploadProgress:
        const percentDone = event.total ? Math.round(100 * event.loaded / event.total) : 0;
        progress(percentDone);
        return `File is ${percentDone}% uploaded.`;

      case HttpEventType.Response:
        return event.body;

      default:
        return `File surprising upload event: ${event.type}.`;
    }
  }
}

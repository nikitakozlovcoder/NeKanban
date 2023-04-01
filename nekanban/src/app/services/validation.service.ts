import {Injectable} from "@angular/core";
import tinymce, {Editor} from "tinymce";
import {ValidationErrors, ValidatorFn} from "@angular/forms";

@Injectable()
export class ValidationService {

  editorMinLengthValidator(editor: Editor|null, minLength: number) : ValidatorFn {
    return (): ValidationErrors | null => {
      if (editor?.initialized) {
        return editor!.getContent({format : 'text'}).length < minLength ? {commentMinLength: true} : null;
      }
      return null;
    };
  }
}

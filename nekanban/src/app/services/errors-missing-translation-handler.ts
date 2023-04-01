import {MissingTranslationHandler, MissingTranslationHandlerParams} from "@ngx-translate/core";

export class ErrorsMissingTranslationHandler implements MissingTranslationHandler {
  handle(params: MissingTranslationHandlerParams) {
    if (params.key.includes("Errors")) {
      return "Неизвестная ошибка";
    }
    return params.key;
  }
}

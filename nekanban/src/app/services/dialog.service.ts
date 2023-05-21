import {Injectable} from "@angular/core";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";
import {ErrorCodes} from "../constants/ErrorCodes";

@Injectable()
export class DialogService {

  constructor(private readonly toastrService: ToastrService,
              private readonly translateService: TranslateService) {
  }

  openToast(err?: string) {
    if (err) {
      let errorCode = err;
      if (!Object.getOwnPropertyNames(ErrorCodes).some(el => el === err)) {
         errorCode = "UnknownError";
      }
      this.translateService.get(`Errors.${errorCode}`).subscribe(result => {
        this.toastrService.error(result);
      });
    }
  }
}

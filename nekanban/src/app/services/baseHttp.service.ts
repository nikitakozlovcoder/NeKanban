﻿import {Injectable} from "@angular/core";
import { environment } from "../../environments/environment";

@Injectable()
export class BaseHttpService {
  baseUrl = environment.baseUrl;
}

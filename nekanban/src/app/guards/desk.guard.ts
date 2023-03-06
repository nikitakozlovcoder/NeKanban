import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {UserStorageService} from "../services/userStorage.service";

@Injectable()
export class DeskGuard implements CanActivate{
  constructor(private router: Router,
              private userStorageService: UserStorageService) {
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.userStorageService.getAccessToken() != null) {
      return true;
    }
    return this.router.parseUrl('/authorization');
  }
}

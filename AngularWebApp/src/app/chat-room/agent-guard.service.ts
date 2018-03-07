import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {map} from 'rxjs/operators';
import {UserService} from '../shared';

@Injectable()
export class AgentGuard implements CanActivate {
  constructor(private router: Router, private userService: UserService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.userService.currentUser$.pipe(map((user) => user.role === 'admin' ? true : false));
  }
}

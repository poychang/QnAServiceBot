import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {Observable} from 'rxjs/Observable';
import {distinctUntilChanged, map} from 'rxjs/operators';
import {ReplaySubject} from 'rxjs/ReplaySubject';

import {User} from '../models';

import {ApiService} from './api.service';

@Injectable()
export class UserService {
  private currentUserSubject = new BehaviorSubject<User>({} as User);
  public currentUser$ = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private apiService: ApiService) {}

  populate() {
    const username =
      document.cookie.replace(/(?:(?:^|.*;\s*)username\s*\=\s*([^;]*).*$)|^.*$/, '$1');

    if (username) {
      this.apiService.get('/user/info')
        .subscribe(data => this.setAuth(data), err => this.purgeAuth());
    } else {
      this.purgeAuth();
    }
  }

  setAuth(user: User) {
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
  }

  purgeAuth() {
    this.apiService.get(`/user/logout`).subscribe(() => {}, () => {});
    this.currentUserSubject.next({} as User);
    this.isAuthenticatedSubject.next(false);
  }

  attemptAuth(credentials): Observable<User> {
    return this.apiService.post('/user/login', credentials).pipe(map(user => {
      this.setAuth(user);
      return user;
    }));
  }
}

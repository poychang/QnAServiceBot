import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {take} from 'rxjs/operators';

import {BotChatService, BotToken, User, UserService} from '../shared';

@Injectable()
export class ChatTokenResolver implements Resolve<BotToken> {
  currentUser: User;
  constructor(private userService: UserService, private botChatService: BotChatService) {
    this.userService.currentUser$.subscribe(user => this.currentUser = user);
  }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<BotToken> {
    return this.botChatService.fetchToken(this.currentUser).pipe(take(1));
  }
}

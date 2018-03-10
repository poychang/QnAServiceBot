import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {map} from 'rxjs/operators';

import {BotToken} from '../models/bot-token.model';
import {User} from '../models/user.model';

import {ApiService} from './api.service';

@Injectable()
export class BotChatService {
  private botChatTokenSubject = new BehaviorSubject<BotToken>({} as BotToken);
  public botChatToken$ = this.botChatTokenSubject.asObservable();

  constructor(private apiService: ApiService) {}

  fetchToken(user: User) {
    return this.apiService.post('/BotToken/generate', '').pipe(map(token => {
      console.log('Direct Line Token: ', token);
      this.botChatTokenSubject.next(token);
      return token;
    }));
  }
}

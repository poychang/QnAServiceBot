import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {BotChatService, User, UserService} from '../shared';

@Component({
  selector: 'app-user-chat-room',
  templateUrl: './user-chat-room.component.html',
  styleUrls: ['./user-chat-room.component.scss']
})
export class UserChatRoomComponent implements OnInit {
  botChatToken = this.route.snapshot.data['botChatToken'].token;
  currentUser: User;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private botChatService: BotChatService) {
    this.userService.currentUser$.subscribe((userData) => { this.currentUser = userData; });
  }

  ngOnInit() {}
}

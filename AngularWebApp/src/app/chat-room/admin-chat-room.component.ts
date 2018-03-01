import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {DirectLine} from 'botframework-directlinejs';

import {BotChatService, BotToken, User, UserService} from '../shared';

@Component({
  selector: 'app-admin-chat-room',
  templateUrl: './admin-chat-room.component.html',
  styleUrls: ['./admin-chat-room.component.scss']
})
export class AdminChatRoomComponent implements OnInit {
  botChatToken = this.route.snapshot.data['botChatToken'].token;
  currentUser: User;
  connected: boolean = false;
  connectState: string = '未連線';
  message: string = '';
  botConnection: DirectLine = new DirectLine({ token: this.botChatToken });

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private botChatService: BotChatService) {
    this.userService.currentUser$.subscribe((userData) => { this.currentUser = userData; });
  }

  ngOnInit() {}

  connect() {
    this.botConnection
      .postActivity({
        type: 'event',
        value: '',
        from: {
          id: this.currentUser.id,
          name: this.currentUser.username,
          iconUrl: this.currentUser.image
        },
        name: this.connected ? 'disconnect' : 'connect'
      })
      .subscribe(data => this.connectionSuccess(data));
  };

  connectionSuccess(data) {
    if (data === 'retry') this.message = '連線失敗，請再試一次';
    console.log('connect success', data);
    this.connected = !this.connected;
    if (this.connected) {
      this.connectState = '已連線';
      this.message = '您已成功連線，使用者可以與您建立連線';
    } else {
      this.connectState = '未連線';
      this.message = '您已斷開連線，請重新連線';
    }
  };

  stopConversation() {
    this.botConnection
      .postActivity({
        type: 'event',
        value: '',
        from: {
          id: this.currentUser.id,
          name: this.currentUser.username,
          iconUrl: this.currentUser.image
        },
        name: 'stopConversation'
      })
      .subscribe(id => console.log('success to disconnect with user'));
  };
}

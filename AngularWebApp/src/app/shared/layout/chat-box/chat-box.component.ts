import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {App, ChatProps, DirectLine} from 'botframework-webchat';

import {User} from '../../models/user.model';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.scss']
})
export class ChatBoxComponent implements OnInit {
  @Input() token: string;
  @Input() currentUser: User;
  @ViewChild('botWindow') botWindowElement: ElementRef;

  constructor() {}

  ngOnInit() {
    let botConnection = new DirectLine({ secret: '', token: this.token, domain: '' });
    let botChatProps: ChatProps = {
      botConnection: botConnection,
      locale: 'zh-hant',
      user: {
        id: this.currentUser.id,
        name: this.currentUser.username,
        iconUrl: this.currentUser.image
      },
      bot: { id: 'bot' },
      resize: 'detect'
    };
    App(botChatProps, this.botWindowElement.nativeElement);
  }
}

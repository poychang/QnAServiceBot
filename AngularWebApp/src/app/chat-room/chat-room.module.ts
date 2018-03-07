import {ModuleWithProviders} from '@angular/compiler/src/core';
import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';

import {SharedModule} from '../shared/shared.module';

import {AgentChatRoomComponent} from './agent-chat-room.component';
import {AgentGuard} from './agent-guard.service';
import {AuthGuard} from './auth-guard.service';
import {ChatTokenResolver} from './chat-token-resolver.service';
import {UserChatRoomComponent} from './user-chat-room.component';

const chatRoomRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'user-chat-room',
    component: UserChatRoomComponent,
    resolve: {
      botChatToken: ChatTokenResolver
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-chat-room',
    component: AgentChatRoomComponent,
    resolve: {
      botChatToken: ChatTokenResolver
    },
    canActivate: [AgentGuard]
  }
]);
@NgModule({
  imports: [chatRoomRouting, RouterModule, SharedModule],
  declarations: [AgentChatRoomComponent, UserChatRoomComponent],
  providers: [AgentGuard, AuthGuard, ChatTokenResolver]
})
export class ChatRoomModule {}

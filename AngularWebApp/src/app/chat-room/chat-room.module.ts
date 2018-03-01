import {ModuleWithProviders} from '@angular/compiler/src/core';
import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';

import {SharedModule} from '../shared/shared.module';

import {AdminChatRoomComponent} from './admin-chat-room.component';
import {AdminGuard} from './admin-guard.service';
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
    component: AdminChatRoomComponent,
    resolve: {
      botChatToken: ChatTokenResolver
    },
    canActivate: [AdminGuard]
  }
]);
@NgModule({
  imports: [chatRoomRouting, RouterModule, SharedModule],
  declarations: [AdminChatRoomComponent, UserChatRoomComponent],
  providers: [AdminGuard, AuthGuard, ChatTokenResolver]
})
export class ChatRoomModule {}

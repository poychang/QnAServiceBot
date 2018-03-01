import {CommonModule} from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import {ModuleWithProviders, NgModule} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';

import {MaterialModule} from './material/material.module';
import {ApiService} from './services/api.service';
import {AuthGuard} from './services/auth-guard.service';
import {BotChatService} from './services/bot-chat.service';
import {UserService} from './services/user.service';
import {ShowAuthedDirective} from './show-authed.directive';

@NgModule({
  imports: [CommonModule, HttpClientModule, MaterialModule, ReactiveFormsModule],
  exports:
    [CommonModule, HttpClientModule, MaterialModule, ReactiveFormsModule, ShowAuthedDirective],
  declarations: [ShowAuthedDirective]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [ApiService, AuthGuard, BotChatService, UserService]
    };
  }
}

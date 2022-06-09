import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ChatItemComponent } from './common/chat-item/chat-item.component';
import { ChatsComponent } from './overlays/chats/chats.component';
import { MessagesComponent } from './overlays/messages/messages.component';
import { MessageItemComponent } from './common/message-item/message-item.component';
import { HomeIndexComponent } from './home-index/home-index.component';
import { ListboxModule } from 'primeng/listbox'
import { BadgeModule } from 'primeng/badge'
import { AvatarModule } from 'primeng/avatar';
import { MessageInputComponent } from './common/message-input/message-input.component'

@NgModule({
  declarations: [
    ChatItemComponent,
    ChatsComponent,
    MessagesComponent,
    MessageItemComponent,
    HomeIndexComponent,
    MessageInputComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule,
    ListboxModule,
    BadgeModule,
    AvatarModule
  ]
})
export class HomeModule { }

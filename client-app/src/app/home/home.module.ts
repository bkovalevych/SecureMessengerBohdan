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
import { MenuModule } from 'primeng/menu';
import { SlideMenuModule } from 'primeng/slidemenu';
import { MessageInputComponent } from './common/message-input/message-input.component'
import { VirtualScrollerModule } from 'primeng/virtualscroller';
import { DialogModule } from 'primeng/dialog'
import { AutoCompleteModule } from 'primeng/autocomplete'
import { CreateChatComponent } from './overlays/create-chat/create-chat.component'

@NgModule({
  declarations: [
    ChatItemComponent,
    ChatsComponent,
    MessagesComponent,
    MessageItemComponent,
    HomeIndexComponent,
    MessageInputComponent,
    CreateChatComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule,
    ListboxModule,
    BadgeModule,
    AvatarModule,
    MenuModule,
    SlideMenuModule,
    VirtualScrollerModule,
    DialogModule,
    AutoCompleteModule
  ]
})
export class HomeModule { }

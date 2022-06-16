import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { firstValueFrom, map, Subscription } from 'rxjs';
import { Paging } from 'src/app/core/models/queries/paging';
import { ChatValue } from 'src/app/core/models/values/chat-value';
import { MessageValue } from 'src/app/core/models/values/message-value';
import { UserValue } from 'src/app/core/models/values/user-value';
import { MessageService } from 'src/app/core/services/api/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styles: [
  ]
})
export class MessagesComponent implements OnInit, OnChanges, OnDestroy {
  @Input() chat: ChatValue;
  @Input() user: UserValue;
  paging: Paging = {skip: 0, take: 50};
  totalCount: number;
  messages: MessageValue[] = [];
  subscriptionForUpdates: Subscription;

  constructor(private messageService: MessageService) { }
  
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat'].currentValue) {
      this.paging = {skip: 0, take: 50};
      let chat: ChatValue = changes['chat'].currentValue
      this.messageService.init(chat.id)
        .then(() => this.loadMessages(chat.id))
        .then(() => this.subscribeForUpdates());
    }
  }

  private loadMessages = (chatId: string) => {
    return firstValueFrom(this.messageService.loadMessages(chatId, this.paging)
    .pipe(map(messages => {
      this.totalCount = messages.totalCount;
      this.paging = { skip: messages.skip, take: messages.take}
      this.messages = messages.items;
    })))
  }

  private subscribeForUpdates = () => {
    if (this.subscriptionForUpdates) {
      this.subscriptionForUpdates.unsubscribe();
    }
    this.subscriptionForUpdates = this.messageService.onUpdatedMessages()
    .subscribe(message => {
      this.messages = [message, ...this.messages];
    })
  }

  isFromMe = (message: MessageValue):boolean => {
    return message.fromId == this.user.id;
  }

  ngOnInit(): void {
    
  }

  ngOnDestroy(): void {
    this.messageService.disconect();
  }
}

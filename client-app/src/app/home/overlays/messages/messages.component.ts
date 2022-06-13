import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Paging } from 'src/app/core/models/queries/paging';
import { ChatValue } from 'src/app/core/models/values/chat-value';
import { MessageValue } from 'src/app/core/models/values/message-value';
import { UserValue } from 'src/app/core/models/values/user-value';
import { AuthService } from 'src/app/core/services/api/auth.service';
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
  
  constructor(private messageService: MessageService) { }
  
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat'].currentValue) {
      this.paging = {skip: 0, take: 50};
      let chat: ChatValue = changes['chat'].currentValue
      this.messageService.init(chat.id);
      this.messageService.loadMessages(chat.id, this.paging)
      .subscribe(messages => {
        this.totalCount = messages.totalCount;
        this.paging = { skip: messages.skip, take: messages.take}
        this.messages = messages.items;
      })
    }
  }

  isFromMe = (message: MessageValue):boolean => {
    return message.fromId == this.user.id;
  }

  ngOnInit(): void {
    this.messageService.updateMessages()
    .subscribe(message => {
      this.messages.unshift(message);
    })
  }

  ngOnDestroy(): void {
    this.messageService.disconect();
  }
}

import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ChatValue } from 'src/app/core/models/values/chat-value';
import { MessageValue } from 'src/app/core/models/values/message-value';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styles: [ 
  ]
})
export class MessagesComponent implements OnInit, OnChanges {
  @Input() chat: ChatValue;
  messages: MessageValue[];
  constructor() { }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chat'].currentValue) {
      this.messages = [...Array(5).keys()].map<MessageValue>(
        val => 
        ({
          id: val.toString(), 
          fromId: val.toString(),
          fromName: changes['chat'].currentValue['name'],
          toId: "",
          toName: "me",
          text: `message n ${val}. from ${changes['chat'].currentValue['name']}`
        }))
    }
  }

  isFromMe = (message: MessageValue):boolean => {
    return parseInt(message.id) % 2 == 0;
  }

  ngOnInit(): void {
  }

}

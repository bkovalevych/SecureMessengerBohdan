import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ChatValue } from 'src/app/core/models/values/chat-value';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styles: [
  ]
})
export class ChatsComponent implements OnInit {
  @Input() chats: ChatValue[];
  @Output() onSelectedChat = new EventEmitter<ChatValue>();
  selectedChat: ChatValue;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  onSelectChanged() {
    if (this.selectedChat) {
      this.onSelectedChat.next(this.selectedChat);
      this.router.navigate([], {
        queryParams: {chatId: this.selectedChat.id},
        queryParamsHandling: 'merge'
      })
    }
  }
}

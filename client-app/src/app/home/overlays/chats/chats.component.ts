import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ChatValue } from 'src/app/core/models/values/chat-value';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styles: [
  ]
})
export class ChatsComponent implements OnInit, OnChanges {
  @Input() chats: ChatValue[];
  @Input() initChat: ChatValue;
  @Output() onSelectedChat = new EventEmitter<ChatValue>();
  selectedChat: ChatValue | null;

  constructor(private router: Router) { }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initChat'].currentValue || (!changes['initChat'].currentValue && changes['initChat'].previousValue)) {
      this.selectedChat = changes['initChat'].currentValue
    }  
  }

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

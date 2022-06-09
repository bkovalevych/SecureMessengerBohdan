import { Component, Input, OnInit } from '@angular/core';
import { ChatValue } from 'src/app/core/models/values/chat-value';

@Component({
  selector: 'app-chat-item',
  templateUrl: './chat-item.component.html',
  styles: [
  ]
})
export class ChatItemComponent implements OnInit {
  @Input() chat: ChatValue;
  constructor() { }

  ngOnInit(): void {
  }

}

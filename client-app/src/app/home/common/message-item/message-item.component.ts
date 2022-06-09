import { Component, Input, OnInit } from '@angular/core';
import { MessageValue } from 'src/app/core/models/values/message-value';

@Component({
  selector: 'app-message-item',
  templateUrl: './message-item.component.html',
  styleUrls: [
    './message-item.component.scss'
  ]
})
export class MessageItemComponent implements OnInit {
  @Input() message: MessageValue;
  @Input() isFromMe: boolean;

  constructor() { }

  ngOnInit(): void {
  }

}

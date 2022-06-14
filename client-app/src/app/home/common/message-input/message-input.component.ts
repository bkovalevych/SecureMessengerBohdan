import { Component, Input, OnInit } from '@angular/core';
import { MessageRequest } from 'src/app/core/models/requests/message-request';
import { MessageService } from 'src/app/core/services/api/message.service';

@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styles: [
  ]
})
export class MessageInputComponent implements OnInit {
  text: string = "";

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
  }
  
  async sendMessage() {
    await this.messageService.sendMessage(this.text);
    this.text = "";
  }
}

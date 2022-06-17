import { Component, Input, OnInit, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CreateChatRequest } from 'src/app/core/models/requests/create-chat-request';
import { UserValue } from 'src/app/core/models/values/user-value';
import { AuthService } from 'src/app/core/services/api/auth.service';
import { ChatService } from 'src/app/core/services/api/chat.service';
import { ChatKeyHelperService } from 'src/app/core/services/security/chat-key-helper.service';

@Component({
  selector: 'app-create-chat',
  templateUrl: 'create-chat.component.html',
  styles: [
  ]
})
export class CreateChatComponent implements OnInit {
  @Input() visible: boolean; 
  @Output() visibleChanged = new EventEmitter<boolean>()
  
  users: UserValue[];
  errors: string[];
  selectedUsers: UserValue[];
  chatName: string;

  constructor(private chatService: ChatService, 
    private auth: AuthService,
    private chatKeyService: ChatKeyHelperService) { }

  async loadUsers(searchValue: string) {
    const users = await firstValueFrom(this.auth.getUsers(searchValue));
    this.users = users;
  }

  cancel() {
    this.selectedUsers = [];
    this.visibleChanged.next(false);
  }

  async save() {
    this.errors = [];
    if(!this.chatName) {
      this.errors.push("Chat name is required");
    }
    if (!this.selectedUsers) {
      this.errors.push("Select at least one user");
    }
    if (this.errors.length == 0) {
      const request: CreateChatRequest = {
        chatName: this.chatName,
        memberIds: this.selectedUsers.map(user => user.id)
      }
      await this.createChat(request);  
      this.visibleChanged.next(false);
    } 
  }

  async createChat(request: CreateChatRequest) {
    const chat = await firstValueFrom(this.chatService.createChat(request));
    const user = await firstValueFrom(this.auth.getUser());
    
    const chatKeys = this.chatKeyService.prepareChatKeys([user, ...this.selectedUsers], chat.id);
    await firstValueFrom(this.chatService.saveEncryptedChatKeys(chatKeys));
  }

  ngOnInit(): void {
  }
  
}

import { Injectable } from '@angular/core';
import { map, Observable, of, Subject } from 'rxjs';
import { Paging } from '../../models/queries/paging';
import { MessageValue } from '../../models/values/message-value';
import * as signalR from '@microsoft/signalr';  
import { environment } from 'src/environments/environment';
import { MessageRequest } from '../../models/requests/message-request';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/values/api-response';
import { PagingList } from '../../models/values/paging-list';
import { ChatKeyHelperService } from '../security/chat-key-helper.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private connection: signalR.HubConnection | null;
  private chatId: string;
  constructor(private http: HttpClient, private chatKeyHelper: ChatKeyHelperService) { }

  loadMessages(idChat: string, paging: Paging) {
    return this.http.get<ApiResponse<PagingList<MessageValue>>>(
      `${environment.baseUrl}/api/chats/${idChat}/messages?skip=${paging.skip}&take=${paging.take}`)
      .pipe(map(it => {
        for (const message of it.result.items) {
          message.text = this.chatKeyHelper.decrypt(message.text);
        }
        return it.result
      }));
  }

  async init(chatId: string) { 
    await this.handleInitChatKey(chatId);
    this.handleInitConnection(chatId);
  }
  
  handleInitChatKey(chatId: string) {
    return this.chatKeyHelper.initChatKey(chatId);
  }

  handleInitConnection(chatId: string) {
    if (!this.connection) {
      let url = `${environment.baseUrl}/messagesHub`;
      this.connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(url, {
          transport: signalR.HttpTransportType.LongPolling,
          accessTokenFactory: () => (localStorage.getItem("secureMessengerAccessToken") || "")
        })
        .withHubProtocol(new signalR.JsonHubProtocol())
        .build();
      
      this.connection.start()
        .then(() => {
          console.log('signalR messages connected')
          this.connection?.invoke("ActivateChat", chatId);
        })
        .catch(err => {
          console.error(err.toString())
        })
    } else {
      if (this.chatId) {
        this.disconectChat(this.chatId);
      }
      this.connection?.invoke("ActivateChat", chatId)
    }
    this.chatId = chatId;
  }

  disconectChat(chatId: string) {
    this.connection?.invoke("DeactivateChat", chatId)  
  }

  disconect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }

  onUpdatedMessages(): Observable<MessageValue> {
    const subject = new Subject<MessageValue>();
    this.connection?.on("ReceiveMessage", args => {
      const message: MessageValue = {...args};
      const text = this.chatKeyHelper.decrypt(message.text);
      message.text = text;
      subject.next(message);
    });

    return subject;
  }

  sendMessage(text: string) {
    const encryptedText = this.chatKeyHelper.encrypt(text);
    let message: MessageRequest = {
      chatId: this.chatId,
      text: encryptedText
    };
    return this.connection?.invoke("SendMessage", message);
  }
}

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

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private connection: signalR.HubConnection | null;
  private chatId: string;
  constructor(private http: HttpClient) { }

  loadMessages(idChat: string, paging: Paging) {
    return this.http.get<ApiResponse<PagingList<MessageValue>>>(
      `${environment.baseUrl}/api/chats/${idChat}/messages?skip=${paging.skip}&take=${paging.take}`)
      .pipe(map(it => it.result));
  }

  init(chatId: string) { 
    if (!this.connection) {
      let url = `${environment.baseUrl}/chatHub`;
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
          console.log('signalR connected')
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

  updateMessages(): Observable<MessageValue> {
    const subject = new Subject<MessageValue>();
    this.connection?.on("ReceiveMessage", args => {
      subject.next(args);
    });
    
    return subject;
  }

  sendMessage(text: string) {
    let message:MessageRequest = {
      chatId: this.chatId,
      text: text
    };
    return this.connection?.invoke("SendMessage", message);
  }
}

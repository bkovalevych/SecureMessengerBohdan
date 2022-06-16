import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../models/values/api-response';
import { ChatValue } from '../../models/values/chat-value';
import { CreateChatRequest } from '../../models/requests/create-chat-request';
import * as signalR from '@microsoft/signalr'

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private connection: signalR.HubConnection | null;
  
  constructor(private http: HttpClient) { }

  loadChats(): Observable<ChatValue[]> {
    return this.http.get<ApiResponse<ChatValue[]>>(`${environment.baseUrl}/api/chats`)
      .pipe(map(it => (it.result)))
  }

  initChats() {
    return this.http.post(`${environment.baseUrl}/api/chats/init`, {});
  }

  initConnection() {
    if (!this.connection) {
      let url = `${environment.baseUrl}/chatsHub`;
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
          console.log('signalR chats connected')
        })
        .catch(err => {
          console.error(err.toString())
        })
    }
  }

  closeConnection() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }

  createChat(request: CreateChatRequest) {
    return this.http.post(`${environment.baseUrl}/api/chats`, request);
  }

  onUpdatedChats() : Observable<ChatValue> {
    const subject = new Subject<ChatValue>();
    this.connection?.on('ChatCreated', (chat: ChatValue) => {
      subject.next(chat);
    })
    return subject;
  }
}

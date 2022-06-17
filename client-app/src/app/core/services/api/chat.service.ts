import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, forkJoin, map, Observable, of, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../models/values/api-response';
import { ChatValue } from '../../models/values/chat-value';
import { CreateChatRequest } from '../../models/requests/create-chat-request';
import * as signalR from '@microsoft/signalr'
import { ChatKeyValue } from '../../models/values/chat-key-value';
import { ChatKeyHelperService } from '../security/chat-key-helper.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private connection: signalR.HubConnection | null;
  
  constructor(private http: HttpClient, private keyHelper: ChatKeyHelperService,
    private auth: AuthService) { }

  loadChats(): Observable<ChatValue[]> {
    return this.http.get<ApiResponse<ChatValue[]>>(`${environment.baseUrl}/api/chats`)
      .pipe(map(it => (it.result)))
  }

  async initChats() {
    let chats = await firstValueFrom(
      this.http.post<ApiResponse<ChatValue[]>>(
        `${environment.baseUrl}/api/chats/init`, {})
        .pipe(map(it => it.result)));
    const user = await firstValueFrom(this.auth.getUser());
    const users = await firstValueFrom(this.auth.getUsers(""));
    users.push(user);
    const observables: Observable<Object>[] = [];
    for (let chat of chats) {
      const selectedUsers = users.filter(u => chat.members?.includes(u.id));
      let keys = this.keyHelper.prepareChatKeys(selectedUsers, chat.id);
      observables.push(this.saveEncryptedChatKeys(keys));
    }
    await firstValueFrom(forkJoin(observables));
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
    return this.http.post<ApiResponse<ChatValue>>(`${environment.baseUrl}/api/chats`, request)
      .pipe(map(it => it.result));
  }

  saveEncryptedChatKeys(encryptedChatKeys: ChatKeyValue[]) {
    return this.http.post(`${environment.baseUrl}/api/chats/saveEncryptedChatKeys`, encryptedChatKeys);
  }

  onUpdatedChats() : Observable<ChatValue> {
    const subject = new Subject<ChatValue>();
    this.connection?.on('ChatCreated', (chat: ChatValue) => {
      subject.next(chat);
    })
    return subject;
  }
}

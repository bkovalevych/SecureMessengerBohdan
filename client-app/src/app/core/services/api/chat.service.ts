import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../models/values/api-response';
import { ChatValue } from '../../models/values/chat-value';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }

  loadChats(): Observable<ChatValue[]> {
    return this.http.get<ApiResponse<ChatValue[]>>(`${environment.baseUrl}/api/chats`)
      .pipe(map(it => (it.result)))
  }

  initChats() {
    return this.http.post(`${environment.baseUrl}/api/chats/init`, {});
  }

  getChatUpdates() : Observable<ChatValue[]> {
    return of<ChatValue[]>([
      new ChatValue({
        countOfUnreadMessages: 0,
        id: "1",
        name: "chat"
      }),
    ])
  }
}

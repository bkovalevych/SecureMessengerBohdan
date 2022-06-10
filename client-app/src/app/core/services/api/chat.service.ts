import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ChatValue } from '../../models/values/chat-value';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor() { }

  loadChats(): Observable<ChatValue[]> {
    return of<ChatValue[]>([
      new ChatValue({
        countOfUnreadMessages: 0,
        id: "1",
        name: "chat"
      }),
    ])
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

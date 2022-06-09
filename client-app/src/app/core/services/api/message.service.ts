import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { Paging } from '../../models/queries/paging';
import { MessageValue } from '../../models/values/message-value';
import * as signalR from '@microsoft/signalr';  
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private connection: signalR.HubConnection;
  constructor() { }

  loadMessages(idChat: string, paging: Paging): Observable<MessageValue[]> {
    return of<MessageValue[]>([

    ])
  }

  init() {
    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.baseUrl)
      .build();

    this.connection.start()
      .then(() => console.log('signalR connected'))
      .catch(err => console.error(err.toString()))
  }

  updateMessages(idChat: string): Observable<MessageValue[]> {
    const subject = new Subject<MessageValue[]>();
    this.connection.on("updateMessage", args => {
      subject.next(args);
    });
    
    return subject;
  }
}

import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ChatValue } from 'src/app/core/models/values/chat-value';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styles: [
  ]
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  @ViewChild('chatsView') chatsView: ElementRef<HTMLDivElement>;
  @ViewChild('messagesView') messagesView: ElementRef<HTMLDivElement>;
  sub: Subscription;

  chats: ChatValue[] = [
    {id: "1", name: "1", countOfUnreadMessages: 0},
    {id: "2", name: "2", countOfUnreadMessages: 0},
    {id: "3", name: "3", countOfUnreadMessages: 0},
    {id: "4", name: "4", countOfUnreadMessages: 3},
    {id: "5", name: "5", countOfUnreadMessages: 0},
  ];
  selectedChat: ChatValue;

  constructor(private activatedRoute: ActivatedRoute) { }

  onSelectedChat(chat: ChatValue) {
    this.selectedChat = chat;
    this.chatsView?.nativeElement.classList.add('d-none');
    this.messagesView?.nativeElement.classList.remove('d-none');
  }

  ngOnInit(): void {
    this.sub = this.activatedRoute.queryParams
      .subscribe(queryParams => {
        if (queryParams['chatId']) {
          this.onSelectedChat({
            id: queryParams['chatId'],
            countOfUnreadMessages: 0,
            name: queryParams['chatId']
          })
        }
      })
  }
  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
  toggleChats() {
    this.chatsView.nativeElement.classList.toggle('d-none');
    this.messagesView.nativeElement.classList.toggle('d-none');
  }
}

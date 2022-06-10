import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { ChatValue } from 'src/app/core/models/values/chat-value';
import { UserValue } from 'src/app/core/models/values/user-value';
import { AuthService } from 'src/app/core/services/api/auth.service';

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
  user: UserValue;
  userLabel:string;
  menuSettings: MenuItem[];

  chats: ChatValue[] = [
    {id: "1", name: "1", countOfUnreadMessages: 0},
    {id: "2", name: "2", countOfUnreadMessages: 0},
    {id: "3", name: "3", countOfUnreadMessages: 0},
    {id: "4", name: "4", countOfUnreadMessages: 3},
    {id: "5", name: "5", countOfUnreadMessages: 0},
  ];
  selectedChat: ChatValue;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService) { }

  onSelectedChat(chat: ChatValue) {
    this.selectedChat = chat;
    this.chatsView?.nativeElement.classList.add('d-none');
    this.messagesView?.nativeElement.classList.remove('d-none');
  }

  ngOnInit(): void {
    this.menuSettings = [
      {
        label: "logout",
        command: () => {
          this.authService.logout();
          this.router.navigateByUrl("/identity/login");
        }
      }
    ]
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
    
    this.authService.getUser()
      .subscribe(user => {
        this.user = user
        this.userLabel = user.email.substring(0, 2);
      });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  toggleChats() {
    this.chatsView.nativeElement.classList.toggle('d-none');
    this.messagesView.nativeElement.classList.toggle('d-none');
  }
}

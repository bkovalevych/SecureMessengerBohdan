import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { first, firstValueFrom, forkJoin, map, Subscription } from 'rxjs';
import { ChatValue } from 'src/app/core/models/values/chat-value';
import { UserValue } from 'src/app/core/models/values/user-value';
import { AuthService } from 'src/app/core/services/api/auth.service';
import { ChatService } from 'src/app/core/services/api/chat.service';
import { RsaHelperService } from 'src/app/core/services/security/rsa-helper.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styles: []
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  @ViewChild('chatsView') chatsView: ElementRef<HTMLDivElement>;
  @ViewChild('messagesView') messagesView: ElementRef<HTMLDivElement>;
  sub: Subscription;
  user: UserValue;
  userLabel:string;
  menuSettings: MenuItem[];

  chats: ChatValue[] = [];
  selectedChat: ChatValue | null;
  createChatModalVisibility = false;

  constructor(
    private rsaHelper: RsaHelperService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private chatService: ChatService,
    private authService: AuthService) { }

  onSelectedChat(chat: ChatValue) {
    this.selectedChat = chat;
    this.chatsView?.nativeElement.classList.add('d-none');
    this.messagesView?.nativeElement.classList.remove('d-none');
  }

  oncreateChatModalVisibilityChanged(visible: boolean) {
    this.createChatModalVisibility = visible;
  }

  ngOnInit(): void {
    this.menuSettings = [
      {
        label: "logout",
        command: () => {
          this.authService.logout();
          this.router.navigateByUrl("/identity/login");
        }
      },
      {
        label: "create chat",
        command: () => {
          this.oncreateChatModalVisibilityChanged(true);
        }
      }
    ]
    forkJoin({user: this.authService.getUser(), chats: this.chatService.loadChats()})
    .pipe(map(({user, chats}) => {
      this.user = user
      this.userLabel = user.email.substring(0, 2);
      this.chats = chats;
      return chats;
    }))
    .pipe(map(this.registerQueryChanges))
    .subscribe()
    this.rsaHelper.initRsaKeys();
    this.registerChatConnection();
  }

  registerChatConnection() {
    this.chatService.initConnection()
    this.chatService.onUpdatedChats()
    .subscribe(chat => {
      this.chats = [chat, ...this.chats]
    });
  }

  registerQueryChanges = (chats: ChatValue[]) => {
    this.sub = this.activatedRoute.queryParams
    .subscribe(queryParams => {
      if (!queryParams['chatId']) {
        return;
      }
      let chat = chats.find(chat => chat.id == queryParams['chatId']);
      if (!chat) {
        return;
      }
      this.onSelectedChat(chat)
    })
  }

  ngOnDestroy(): void {
    this.chatService.closeConnection();
    this.sub.unsubscribe();
  }

  toggleChats() {
    this.router.navigate([], {
      queryParams: {
        chatId: null
      },
      queryParamsHandling: "merge"
    })
    this.selectedChat = null;
    this.chatsView.nativeElement.classList.toggle('d-none');
    this.messagesView.nativeElement.classList.toggle('d-none');
  }

  async initChats() {
    await firstValueFrom(this.chatService.initChats())
    this.chats = await firstValueFrom(this.chatService.loadChats())
  }


}

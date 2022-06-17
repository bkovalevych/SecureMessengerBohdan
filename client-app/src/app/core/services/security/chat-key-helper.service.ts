import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as forgeLib  from 'node-forge';
import { environment } from 'src/environments/environment';
import { GetKeyForChatRequest } from '../../models/requests/get-key-for-chat-request';
import { ChatKeyValue } from '../../models/values/chat-key-value';
import { RsaHelperService } from './rsa-helper.service';
import { ApiResponse } from '../../models/values/api-response';
import { firstValueFrom, map } from 'rxjs';
import { UserValue } from '../../models/values/user-value';

@Injectable({
  providedIn: 'root'
})
export class ChatKeyHelperService {
  private forge: any = forgeLib; 
  
  constructor(private rsaHelper: RsaHelperService, private http: HttpClient) { }

  private get chatKey () {
    return sessionStorage.getItem("chatKey") || "";
  }
  private set chatKey (key: string) {
    sessionStorage.setItem("chatKey", key);
  }

  private get iv () {
    return sessionStorage.getItem("iv") || "";
  }
  private set iv (key: string) {
    sessionStorage.setItem("iv", key);
  }

  async initChatKey(chatId: string) {
    const request : GetKeyForChatRequest = {
      chatId: chatId,
      publicRSAKey: this.rsaHelper.publicKey
    } 
    let chatKeyValue = await firstValueFrom(
      this.http.post<ApiResponse<ChatKeyValue>>(`${environment.baseUrl}/api/chats/key`, request)
      .pipe(map(it => it.result)));
    
    this.chatKey = this.decryptKey(chatKeyValue.key)
    this.iv = this.decryptKey(chatKeyValue.iv); 
  }

  private decryptKey(key: string): string {
    const decryptedBytes = this.rsaHelper.decrypt(key);
    const decrypted = this.forge.util.encode64(decryptedBytes);
    return decrypted;
  }

  decrypt(encrypted64: string) {
    const key =  this.forge.util.decode64(this.chatKey);
    const iv = this.forge.util.decode64(this.iv);
    const encryptedBytes = this.forge.util.decode64(encrypted64);
    const buffer = this.forge.util.createBuffer(encryptedBytes);
    const decipher = this.forge.cipher.createDecipher(
      'AES-CBC',
      key,
    );
    decipher.start({iv});
    decipher.update(buffer);
    decipher.finish();
    
    const decrypted = decipher.output.getBytes();
    const rawValue = decodeURIComponent(decrypted.toString('utf-8'));
    return rawValue;
  }

  encrypt(data: string) {
    const key =  this.forge.util.decode64(this.chatKey);
    const iv = this.forge.util.decode64(this.iv);
    const dataBytes = this.forge.util.createBuffer(encodeURIComponent(data), "utf-8");
    const cipher = this.forge.aes.createEncryptionCipher(key, 'CBC');
    cipher.start(iv);
    cipher.update(dataBytes);
    cipher.finish();
    
    const encryptedBytes = cipher.output.getBytes();
    const encrypted64 = this.forge.util.encode64(encryptedBytes);
    return encrypted64;
  }

  prepareChatKeys(members: UserValue[], chatId: string) {
    const {iv, key} = this.generateChatKey();
    const result = members.map<ChatKeyValue>(
      member => {
        const keys = this.rsaHelper
        .encryptAesKey(iv, key, member.publicKey as string);
        return {
          chatId: chatId,
          iv: keys.aesEncryptedIv,
          key: keys.aesEncryptedKey,
          userId: member.id
        }
    })
    return result;
  }

  private generateChatKey() {
    var key = this.forge.random.getBytesSync(32);
    var iv = this.forge.random.getBytesSync(16);
    return {key, iv};
  }


}

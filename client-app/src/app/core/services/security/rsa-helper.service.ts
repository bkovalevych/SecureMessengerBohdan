import { Injectable } from '@angular/core';
import * as forgeLib from 'node-forge';

@Injectable({
  providedIn: 'root'
})
export class RsaHelperService {
  private forge: any = forgeLib; 
  
  constructor() { }

  get publicKey () {
    return localStorage.getItem("SecureChatBohdanPublicRsaKey") || "";
  }
  set publicKey (key: string) {
    localStorage.setItem("SecureChatBohdanPublicRsaKey", key);
  }

  private get privateKey () {
    return localStorage.getItem("SecureChatBohdanPrivateRsaKey") || "";
  }
  private set privateKey (key: string) {
    localStorage.setItem("SecureChatBohdanPrivateRsaKey", key);
  }

  initRsaKeys() {
    if (this.privateKey && this.publicKey) {
      return;
    }
    let pki = this.forge.pki;
    let rsa = this.forge.pki.rsa;
    let keypair = rsa.generateKeyPair({bits: 2048, e: 0x10001});
    let pubKeyPEM = pki.publicKeyToPem(keypair.publicKey);
    let privKeyPEM = pki.privateKeyToPem(keypair.privateKey);
    
    this.publicKey = pubKeyPEM;
    this.privateKey = privKeyPEM;
  }

  decrypt(encrypted: string) {
    const privateKey = this.forge.pki.privateKeyFromPem(this.privateKey);
    const encryptedRawValue = this.forge.util.decode64(encrypted);
    return privateKey.decrypt(encryptedRawValue);
  }
  
  encryptAesKey(aesIv: any, aesKey: any, publicRsaPemKey: string) {
    const publicRsaKey = this.forge.pki.publicKeyFromPem(publicRsaPemKey);
    const aesEncryptedKey = this.forge.util.encode64(
      publicRsaKey.encrypt(aesKey));
    const aesEncryptedIv = this.forge.util.encode64(
      publicRsaKey.encrypt(aesIv));
    return {aesEncryptedIv, aesEncryptedKey};
  }
}

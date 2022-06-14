import { Injectable } from '@angular/core';
import * as forgeLib from 'node-forge';

@Injectable({
  providedIn: 'root'
})
export class RsaHelperService {
  private forge: any = forgeLib; 
  
  constructor() { }

  get publicKey () {
    return sessionStorage.getItem("publicRsaKey") || "";
  }
  set publicKey (key: string) {
    sessionStorage.setItem("publicRsaKey", key);
  }

  private get privateKey () {
    return sessionStorage.getItem("privateRsaKey") || "";
  }
  private set privateKey (key: string) {
    sessionStorage.setItem("privateRsaKey", key);
  }

  initRsaKeys() {
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
}

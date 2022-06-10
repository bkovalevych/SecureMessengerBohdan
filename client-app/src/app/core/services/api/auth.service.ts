import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../../models/requests/login-request';
import { TokenValue } from '../../models/values/token-value';
import { ApiResponse } from '../../models/values/api-response';
import { UserValue } from '../../models/values/user-value';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }
  
  isLoggedIn() {
    return !!this.accessToken;
  }
  
  logout() {
    this.accessToken = "";
    this.refreshToken = "";
  }

  refresh() {
    const accessToken = this.accessToken;  
    const refreshToken = this.refreshToken;
    
    return this.http.post<ApiResponse<TokenValue>>(`${environment.baseUrl}/api/identity/refresh`, {
      accessToken: accessToken,
      refreshToken: refreshToken
    })
    .pipe(map(this.mapResponseToResult))
    .pipe(map(this.saveTokensAfterRequest))
  }

  login(loginRequest: LoginRequest) {
    return this.http.post<ApiResponse<TokenValue>>(`${environment.baseUrl}/api/identity/login`, loginRequest)
    .pipe(map(this.mapResponseToResult))
    .pipe(map(this.saveTokensAfterRequest));
  }
  
  getUser() {
    return this.http
    .get<ApiResponse<UserValue>>(`${environment.baseUrl}/api/identity`)
    .pipe(map(it => it.result));
  }

  private mapResponseToResult(response: ApiResponse<TokenValue>) {
    return response.result;
  }

  private saveTokensAfterRequest = (result: TokenValue) => {
    this.accessToken = result.accessToken;
    this.refreshToken = result.refreshToken;
    return result;
  }

  public get accessToken() { return localStorage.getItem("secureMessengerAccessToken") || ""};
  public set accessToken(value: string) {localStorage.setItem("secureMessengerAccessToken", value)};

  public get refreshToken() { return localStorage.getItem("secureMessengerRefreshToken") || ""};
  public set refreshToken(value: string) { localStorage.setItem("secureMessengerRefreshToken", value)};
}


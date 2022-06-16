import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { first, firstValueFrom, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../../models/requests/login-request';
import { TokenValue } from '../../models/values/token-value';
import { ApiResponse } from '../../models/values/api-response';
import { UserValue } from '../../models/values/user-value';
import jwtDecode, { JwtPayload,  } from 'jwt-decode';
import { RegisterRequest } from '../../models/requests/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }
  
  async isLoggedIn() {
    let payload : JwtPayload = jwtDecode(this.accessToken);
    let ms = 1000;
    let expired = (payload.exp || 0) * ms;
    if (new Date().valueOf() > expired) {
      try {
        await firstValueFrom(this.refresh())
      } catch(e) {
        return false;
      }
    }
    return this.accessToken.length > 0;
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

  register(registerRequest: RegisterRequest) {
    return this.http.post(`${environment.baseUrl}/api/identity/register`, registerRequest);
  }
  
  getUser() {
    return this.http
    .get<ApiResponse<UserValue>>(`${environment.baseUrl}/api/identity`)
    .pipe(map(it => it.result));
  }

  getUsers(searchValue: string) {
    return this.http
    .get<ApiResponse<UserValue[]>>(`${environment.baseUrl}/api/identity/users?search=${searchValue}`)
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


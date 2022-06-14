import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../api/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanLoad {

  constructor(private router: Router,
    private auth: AuthService) { }
  
  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.handle();
  }

  async handle() {
    try {
      const it = await this.auth.isLoggedIn()
      if (it) {
        return true;
      }
      this.router.navigate(['/identity/login']);
      return false;
    } catch(err) {
      this.router.navigate(['/identity/login']);
      return false;
    }
  }
}

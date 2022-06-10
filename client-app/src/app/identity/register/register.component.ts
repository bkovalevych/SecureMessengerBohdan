import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html',
  styles: [
  ]
})
export class RegisterComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }
  navigateToLogin() {
    this.router.navigate(['/identity/login'], {
      queryParamsHandling: 'merge'
    })
  }
}

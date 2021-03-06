import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnChanges, OnInit, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginRequest } from 'src/app/core/models/requests/login-request';
import { EmptyApiResponse } from 'src/app/core/models/values/api-response';
import { AuthService } from 'src/app/core/services/api/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [ ``
  ],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  email: string | null;
  password: string | null;
  errors: string[];
  form: FormGroup;
  
  constructor(private router: Router, 
    private formBuilder: FormBuilder, 
    private activatedRoute: ActivatedRoute,
    private auth: AuthService) { }
  
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    })
    let {email, password} = history.state;
    if (email && password) {
      this.form.setValue({email, password});
      delete history.state.password;
      delete history.state.email;
    }
  }

  
  login() {
    if (this.form.invalid) {
      this.form.markAsTouched();
      return;
    }
    this.errors = [];
    const loginRequest: LoginRequest = this.form.value;
    this.auth.login(loginRequest)
    .subscribe({next:() => {
      const url = this.activatedRoute.snapshot.queryParams['returnUrl'] || '';
      this.router.navigate([url], {
        queryParams: {returnUrl: null},
        queryParamsHandling: 'merge'
      });
    }, error: (resp: EmptyApiResponse) => {
      this.errors = resp.errors
    }})
  }

  navigateToRegister() {
    this.router.navigate(['/identity/register'], {
      queryParamsHandling: 'merge'
    })
  }
}

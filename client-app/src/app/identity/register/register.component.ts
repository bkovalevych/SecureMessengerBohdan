import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterRequest } from 'src/app/core/models/requests/register-request';
import { EmptyApiResponse } from 'src/app/core/models/values/api-response';
import { AuthService } from 'src/app/core/services/api/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html',
  styles: [
  ]
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  errors: string[] = [];

  constructor(
    private auth: AuthService,
    private router: Router, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      passwords: this.formBuilder.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['']
      }, {
        validators: (control: AbstractControl): {passwordMismatch: boolean} | null => {
          if (control.get("password")?.value != control.get("confirmPassword")?.value) {
            return {passwordMismatch: true};
          }
          return null;
        }
      })
    })
  }

  navigateToLogin() {
    this.router.navigate(['/identity/login'], {
      queryParamsHandling: 'merge'
    })
  }

  register() {
    if (this.form.invalid) {
      this.form.markAsTouched();
      return;
    }
    this.errors = [];
    const request: RegisterRequest = this.form.value;
    request.password = this.form.get("passwords")?.get("password")?.value;
    this.auth.register(request).subscribe({
      next: () => {
        this.router.navigate(['/identity/login'], {state: {email: request.email, password: request.password}})
      },
      error: (resp: EmptyApiResponse) => {
        this.errors = resp.errors
      }
    });
  }
}

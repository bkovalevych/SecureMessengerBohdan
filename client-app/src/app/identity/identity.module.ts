import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IdentityRoutingModule } from './identity-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CardModule } from 'primeng/card'
import { PanelModule } from 'primeng/panel'
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CardModule,
    CommonModule,
    SharedModule,
    IdentityRoutingModule,
    PanelModule
  ]
})
export class IdentityModule { }

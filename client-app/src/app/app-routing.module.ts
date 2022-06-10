import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuardService } from './core/services/guards/auth-guard.service';

const routes: Routes = [
  {
    path: '', pathMatch: 'full', canActivate: [AuthGuardService], 
    component: AppComponent
  },
  {
    path: "identity", loadChildren: () => import("./identity/identity.module").then(it => it.IdentityModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [
    RouterModule,
  ]
})
export class AppRoutingModule { }

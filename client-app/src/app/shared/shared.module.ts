import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PanelMenuModule } from 'primeng/panelmenu'
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ButtonModule,
    InputTextModule,
    PanelMenuModule,
    FormsModule
  ],
  exports: [
    ButtonModule,
    InputTextModule,
    PanelMenuModule,
    FormsModule
  ]
})
export class SharedModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PanelMenuModule } from 'primeng/panelmenu'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ButtonModule,
    InputTextModule,
    PanelMenuModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    ButtonModule,
    InputTextModule,
    PanelMenuModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }

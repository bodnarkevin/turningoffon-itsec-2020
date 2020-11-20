import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { StartpageRoutingModule } from './startpage-routing.module';
import { StartpageComponent } from './startpage.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [StartpageComponent, LoginComponent, RegisterComponent],
  imports: [
    CommonModule,
    StartpageRoutingModule,
    ReactiveFormsModule
  ]
})
export class StartpageModule { }

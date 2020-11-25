import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { StartpageRoutingModule } from './startpage-routing.module';
import { StartpageComponent } from './startpage.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [StartpageComponent, LoginComponent, RegisterComponent],
  imports: [
    CommonModule,
    StartpageRoutingModule,
    ReactiveFormsModule,
    MatSnackBarModule
  ]
})
export class StartpageModule { }

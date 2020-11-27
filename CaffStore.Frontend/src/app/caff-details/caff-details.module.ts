import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CaffDetailsRoutingModule } from './caff-details-routing.module';
import { CaffDetailsComponent } from './caff-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
  declarations: [CaffDetailsComponent],
  imports: [
    CommonModule,
    CaffDetailsRoutingModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatTooltipModule,
    MatSnackBarModule
  ]
})
export class CaffDetailsModule { }

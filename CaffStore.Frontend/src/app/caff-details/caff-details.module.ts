import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CaffDetailsRoutingModule } from './caff-details-routing.module';
import { CaffDetailsComponent } from './caff-details.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [CaffDetailsComponent],
  imports: [
    CommonModule,
    CaffDetailsRoutingModule,
    ReactiveFormsModule
  ]
})
export class CaffDetailsModule { }

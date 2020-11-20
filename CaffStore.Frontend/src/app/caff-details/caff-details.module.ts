import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CaffDetailsRoutingModule } from './caff-details-routing.module';
import { CaffDetailsComponent } from './caff-details.component';


@NgModule({
  declarations: [CaffDetailsComponent],
  imports: [
    CommonModule,
    CaffDetailsRoutingModule
  ]
})
export class CaffDetailsModule { }

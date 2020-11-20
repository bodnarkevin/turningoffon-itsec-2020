import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CaffListRoutingModule } from './caff-list-routing.module';
import { CaffListComponent } from './caff-list.component';


@NgModule({
  declarations: [CaffListComponent],
  imports: [
    CommonModule,
    CaffListRoutingModule
  ]
})
export class CaffListModule { }

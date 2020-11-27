import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CaffListRoutingModule } from './caff-list-routing.module';
import { CaffListComponent } from './caff-list.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [CaffListComponent],
  imports: [
    CommonModule,
    CaffListRoutingModule,
    SharedModule
  ]
})
export class CaffListModule { }

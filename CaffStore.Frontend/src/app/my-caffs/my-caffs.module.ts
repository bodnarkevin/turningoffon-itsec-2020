import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyCaffsRoutingModule } from './my-caffs-routing.module';
import { MyCaffsComponent } from './my-caffs.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [MyCaffsComponent],
  imports: [
    CommonModule,
    MyCaffsRoutingModule,
    SharedModule
  ]
})
export class MyCaffsModule { }

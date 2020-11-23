import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { CaffListRoutingModule } from './caff-list-routing.module';
import { CaffListComponent } from './caff-list.component';


@NgModule({
  declarations: [CaffListComponent],
  imports: [
    CommonModule,
    CaffListRoutingModule,
    MatCardModule,
    MatButtonModule
  ]
})
export class CaffListModule { }

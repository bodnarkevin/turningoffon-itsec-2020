import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CaffListRoutingModule } from './caff-list-routing.module';
import { CaffListComponent } from './caff-list.component';
import { MatTooltipModule } from '@angular/material/tooltip';



@NgModule({
  declarations: [CaffListComponent],
  imports: [
    CommonModule,
    CaffListRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule
  ]
})
export class CaffListModule { }

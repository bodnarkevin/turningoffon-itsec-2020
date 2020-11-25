import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';

import { SharedCaffListComponent } from './components/shared-caff-list/shared-caff-list.component';
import { NewCaffDialogComponent } from './components/new-caff-dialog/new-caff-dialog.component';


@NgModule({
  declarations: [SharedCaffListComponent, NewCaffDialogComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatTooltipModule,
    MatDialogModule,
    ReactiveFormsModule
  ],
  exports: [
    SharedCaffListComponent
  ]
})
export class SharedModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CaffListComponent } from './caff-list.component';

const routes: Routes = [{ path: '', component: CaffListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaffListRoutingModule { }

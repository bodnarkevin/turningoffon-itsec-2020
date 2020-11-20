import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CaffDetailsComponent } from './caff-details.component';

const routes: Routes = [{ path: '', component: CaffDetailsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaffDetailsRoutingModule { }

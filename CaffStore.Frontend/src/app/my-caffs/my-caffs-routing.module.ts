import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../auth/auth-guard';
import { MyCaffsComponent } from './my-caffs.component';

const routes: Routes = [{ path: '', component: MyCaffsComponent, canActivate: [AuthGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyCaffsRoutingModule { }

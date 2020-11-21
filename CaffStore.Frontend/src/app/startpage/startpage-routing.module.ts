import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../auth/auth-guard';
import { StartpageComponent } from './startpage.component';

const routes: Routes = [{ path: '', component: StartpageComponent, canActivate: [AuthGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StartpageRoutingModule { }

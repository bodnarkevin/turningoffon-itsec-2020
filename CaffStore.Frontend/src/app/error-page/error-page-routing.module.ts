import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../auth/auth-guard';
import { ErrorPageComponent } from './error-page.component';

const routes: Routes = [{ path: '', component: ErrorPageComponent, canActivate: [AuthGuard]  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ErrorPageRoutingModule { }

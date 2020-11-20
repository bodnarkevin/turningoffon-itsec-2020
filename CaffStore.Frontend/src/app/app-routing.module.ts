import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: () => import('./startpage/startpage.module').then(m => m.StartpageModule) },
  { path: 'profile', loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule) },
  { path: 'caff', loadChildren: () => import('./caff-details/caff-details.module').then(m => m.CaffDetailsModule) },
  { path: 'list', loadChildren: () => import('./caff-list/caff-list.module').then(m => m.CaffListModule) },
  { path: 'my_caffs', loadChildren: () => import('./caff-list/caff-list.module').then(m => m.CaffListModule) },
  { path: 'users', loadChildren: () => import('./users/users.module').then(m => m.UsersModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

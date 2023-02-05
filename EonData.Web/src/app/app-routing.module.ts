import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';

// application level routing definition
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'clouds', loadChildren: () => import('./modules/cloud-control/cloud-control.module').then(m => m.CloudControlModule) }
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

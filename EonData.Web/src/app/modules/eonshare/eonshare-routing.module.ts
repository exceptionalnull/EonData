import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EonshareComponent } from './components/eonshare/eonshare.component';

const routes: Routes = [
  { path: '', component: EonshareComponent },
  { path: ':objectKey', component: EonshareComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EonshareRoutingModule { }

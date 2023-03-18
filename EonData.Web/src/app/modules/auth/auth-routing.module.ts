import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { CallbackComponent } from './components/callback/callback.component';

const routes: Routes = [
  { path: '', component: CallbackComponent }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CloudControlRoutingModule { }

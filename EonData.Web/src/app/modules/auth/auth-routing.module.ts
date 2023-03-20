import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { CallbackComponent } from './components/callback/callback.component';
import { ProfileComponent } from './components/profile/profile.component';

const routes: Routes = [
  { path: '', component: CallbackComponent },
  { path: 'profile', component: ProfileComponent }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CloudControlRoutingModule { }

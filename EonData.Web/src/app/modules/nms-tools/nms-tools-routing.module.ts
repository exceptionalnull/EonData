import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CookbookComponent } from './components/cookbook/cookbook.component';

const routes: Routes = [
  { path: 'cookbook', component: CookbookComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NmsToolsRoutingModule { }

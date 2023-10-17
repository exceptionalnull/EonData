import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RecipeWizardComponent } from './components/recipe-wizard/recipe-wizard.component';

const routes: Routes = [
  { path: 'wizard', component: RecipeWizardComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NmsToolsRoutingModule { }

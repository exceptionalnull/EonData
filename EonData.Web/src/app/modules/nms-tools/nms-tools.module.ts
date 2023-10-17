import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NmsToolsRoutingModule } from './nms-tools-routing.module';
import { RecipeWizardComponent } from './components/recipe-wizard/recipe-wizard.component';


@NgModule({
  declarations: [
    RecipeWizardComponent
  ],
  imports: [
    CommonModule,
    NmsToolsRoutingModule
  ]
})
export class NmsToolsModule { }

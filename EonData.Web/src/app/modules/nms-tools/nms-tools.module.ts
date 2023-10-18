import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NmsToolsRoutingModule } from './nms-tools-routing.module';
import { RecipeWizardComponent } from './components/recipe-wizard/recipe-wizard.component';
import { FormsModule } from '@angular/forms';
import { NmsDataService } from './services/nms-data.service';
import { RecipeNodeComponent } from './components/recipe-node/recipe-node.component';


@NgModule({
  declarations: [
    RecipeWizardComponent,
    RecipeNodeComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    NmsToolsRoutingModule
  ],
  providers: [NmsDataService]
})
export class NmsToolsModule { }

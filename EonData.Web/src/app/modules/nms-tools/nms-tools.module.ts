import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NmsToolsRoutingModule } from './nms-tools-routing.module';
import { CookbookComponent } from './components/cookbook/cookbook.component';


@NgModule({
  declarations: [
    CookbookComponent
  ],
  imports: [
    CommonModule,
    NmsToolsRoutingModule
  ]
})
export class NmsToolsModule { }

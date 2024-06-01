import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EonshareRoutingModule } from './eonshare-routing.module';
import { EonShareComponent } from './components/eonshare/eonshare.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    EonShareComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    EonshareRoutingModule
  ]
})
export class EonshareModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EonshareRoutingModule } from './eonshare-routing.module';
import { EonShareComponent } from './components/eonshare/eonshare.component';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [
    EonShareComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    EonshareRoutingModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatTooltipModule
  ]
})
export class EonshareModule { }

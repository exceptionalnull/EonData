import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxFilesizeModule } from 'ngx-filesize';
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
    NgxFilesizeModule,
    EonshareRoutingModule
  ]
})
export class EonshareModule { }

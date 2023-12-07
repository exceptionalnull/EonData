import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxFilesizeModule } from 'ngx-filesize';
import { EonshareRoutingModule } from './eonshare-routing.module';
import { EonshareComponent } from './components/eonshare/eonshare.component';


@NgModule({
  declarations: [
    EonshareComponent
  ],
  imports: [
    CommonModule,
    NgxFilesizeModule,
    EonshareRoutingModule
  ]
})
export class EonshareModule { }

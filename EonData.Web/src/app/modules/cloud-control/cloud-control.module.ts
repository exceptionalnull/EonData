import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CloudControlRoutingModule } from './cloud-control-routing.module';


@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    CloudControlRoutingModule
  ],
  exports:[CloudControlRoutingModule]
})
export class CloudControlModule { }

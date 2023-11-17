import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { MessageListComponent } from './components/message-list/message-list.component';

const routes: Routes = [
  { path: 'view', component: MessageListComponent }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactFormRoutingModule { }

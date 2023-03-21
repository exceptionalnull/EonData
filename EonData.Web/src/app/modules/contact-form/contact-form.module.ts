import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageListComponent } from './components/message-list/message-list.component';
import { SendMessageFormComponent } from './components/send-message-form/send-message-form.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    MessageListComponent,
    SendMessageFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    SendMessageFormComponent
  ]
})
export class ContactFormModule { }

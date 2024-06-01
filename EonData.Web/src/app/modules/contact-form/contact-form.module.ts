import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageListComponent } from './components/message-list/message-list.component';
import { SendMessageFormComponent } from './components/send-message-form/send-message-form.component';
import { FormsModule } from '@angular/forms';
import { NewMessageNotificationComponent } from './components/new-message-notification/new-message-notification.component';
import { ContactFormRoutingModule } from './contact-form-routing.module';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';

@NgModule({
  declarations: [
    MessageListComponent,
    SendMessageFormComponent,
    NewMessageNotificationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ContactFormRoutingModule,
    ButtonModule,
    InputTextModule,
    InputTextareaModule
  ],
  exports: [
    SendMessageFormComponent,
    NewMessageNotificationComponent
  ]
})
export class ContactFormModule { }

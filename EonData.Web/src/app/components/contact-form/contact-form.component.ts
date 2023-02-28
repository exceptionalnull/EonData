import { Component, Input } from '@angular/core';
import { ContactMessageModel } from '../../models/ContactMessageModel';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent {
  @Input() source: string = '';

  model = new ContactMessageModel();


  sendMessage() {
    this.model.source = this.source;
  }
}

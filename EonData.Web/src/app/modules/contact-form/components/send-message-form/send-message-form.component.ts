import { Component, Input, OnInit } from '@angular/core';
import { ContactMessageModel } from '../../models/ContactMessageModel';
import { ContactService } from '../../contact-form.service';

@Component({
  selector: 'app-contact-send-message',
  templateUrl: './send-message-form.component.html',
  styleUrls: ['./send-message-form.component.scss']
})
export class SendMessageFormComponent implements OnInit {
  @Input() source: string = '';
  public model!: ContactMessageModel;

  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm() {
    this.model = new ContactMessageModel();
    this.model.formSource = this.source;
  }

  sendMessage() {
    this.contactService.submitContactForm(this.model).subscribe(
      response => {
        console.log("contact message sent successfully");
        this.resetForm();
      },
      error => {
        console.log("error sending contact message:", error);
      });
  }

}

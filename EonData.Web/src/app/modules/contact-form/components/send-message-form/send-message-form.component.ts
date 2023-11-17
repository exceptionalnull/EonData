import { Component, Input, OnInit } from '@angular/core';
import { ContactService } from '../../contact-form.service';
import { SendMessageModel } from '../../models/SendMessageModel';

@Component({
  selector: 'app-contact-send-message',
  templateUrl: './send-message-form.component.html',
  styleUrls: ['./send-message-form.component.scss']
})
export class SendMessageFormComponent implements OnInit {
  @Input() source: string = '';
  public model!: SendMessageModel;

  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm() {
    this.model = new SendMessageModel(this.source);
  }

  sendMessage() {
    this.contactService.submitContactForm(this.model).subscribe(
      response => {
        console.log(response);
        this.resetForm();
      },
      error => {
        console.log("error sending contact message:", error);
      });
  }

}

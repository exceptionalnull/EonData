import { Component, Input, OnInit } from '@angular/core';
import { ContactMessageModel } from '../../models/ContactMessageModel';
import { ContactService } from '../../services/ContactService';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent implements OnInit{
  @Input() source: string = '';
  public model!: ContactMessageModel;

  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm() {
    this.model = new ContactMessageModel();
    this.model.source = this.source;
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

import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../contact-form.service';
import { MessageListModel } from '../../models/MessageListModel';
import { ContactMessageModel } from '../../models/ContactMessageModel';

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.scss']
})
export class MessageListComponent implements OnInit {
  public model?: MessageListModel[];
  public readMessage?: ContactMessageModel;
  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.contactService.getMessageList().subscribe(response => { this.model = response; })
  }

  getRowClass(message: MessageListModel) {
    return (message.isRead) ? "unread" : "read";
  }

  selectMessage(messageId: string) {
    this.contactService.getMessage(messageId).subscribe(response => {
      if (response != null) {
        if (!response.isRead) {
          this.markAsRead(messageId);
          response.isRead = true;
        }
        this.readMessage = response;
      }
    });
  }

  markAsRead(messageId: string) {
    this.contactService.setRead(messageId).subscribe(() => {
      const message = this.model?.find(m => m.messageId === messageId);
      if (message) {
        message.isRead = true;
      }
    });
  }
}

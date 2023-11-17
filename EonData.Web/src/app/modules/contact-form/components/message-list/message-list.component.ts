import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../contact-form.service';
import { MessageListModel } from '../../models/MessageListModel';
import { ContactMessageModel } from '../../models/ContactMessageModel';
import { MessageListResponseModel } from '../../models/MessageListResponseModel';

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.scss']
})
export class MessageListComponent implements OnInit {
  public model?: MessageListResponseModel;
  public readMessage?: ContactMessageModel;
  public unreadFilter: string = "none";
  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.updateData();
  }

  updateData() {
    this.readMessage = undefined;
    let unread: boolean | undefined;
    if (this.unreadFilter === "true") {
      unread = true;
    }
    else if (this.unreadFilter === "false") {
      unread = false;
    }
    this.contactService.getMessageList(unread).subscribe(response => { this.model = response; })
  }

  getRowClass(message: MessageListModel) {
    return {
      'list-unread': !message.isRead,
      'list-read': message.isRead,
      'list-selected': message.messageId === this.readMessage?.messageId
    }
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
      const message = this.model?.messages?.find(m => m.messageId === messageId);
      if (message) {
        message.isRead = true;
      }
    });
  }
}

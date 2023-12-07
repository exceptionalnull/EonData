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
  public messages: MessageListModel[] = [];
  public readMessage?: ContactMessageModel;
  public unreadFilter: string = "unread";
  public currentPage: number = 1;
  public finalPage: boolean = true;
  public pageMessages: MessageListModel[] = [];

  private readonly pageLimit: number = 15;

  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.updateData();
    this.pageMessages = this.getPageMessages();
  }

  updateData(pageKey?: string) {
    this.readMessage = undefined;

    let unread: boolean | undefined;
    if (this.unreadFilter === "read") {
      unread = false;
    }
    else if (this.unreadFilter === "unread") {
      unread = true;
    }
    
    this.contactService.getMessageList(unread, pageKey).subscribe(response => {
      console.log(response);
      this.messages = response;
      this.currentPage = 1;
      this.pageMessages = this.getPageMessages();
    })
  }

  getPageMessages(): MessageListModel[] {
    const startIndex: number = (this.currentPage - 1) * this.pageLimit;
    const endIndex: number = Math.min(startIndex + this.pageLimit - 1, this.messages.length);
    this.finalPage = (endIndex == this.messages.length);
    console.log(startIndex);
    console.log(endIndex);
    return this.messages.slice(startIndex, endIndex);
  }

  nextPage() {
    if (!this.finalPage) {
      this.currentPage++;
      this.pageMessages = this.getPageMessages();
    }
  }

  setFilter() {
    this.updateData();
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.pageMessages = this.getPageMessages();
    }
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
      const message = this.messages?.find(m => m.messageId === messageId);
      if (message) {
        message.isRead = true;
      }
    });
  }
}

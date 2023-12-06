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
  public unreadFilter: string = "all";
  public currentPageKey?: string;
  public isFirstPage: boolean = true;
  public pageKeys: string[] = [];
  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.updateData();
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
    
    this.isFirstPage = (pageKey === undefined);
    
    this.contactService.getMessageList(unread, pageKey).subscribe(response => { this.model = response; })
  }

  nextPage() {
    if (this.currentPageKey !== undefined) {
      this.pageKeys.push(this.currentPageKey);
    }
    this.currentPageKey = this.model?.startKey;
    this.updateData(this.model?.startKey)
  }

  setFilter() {
    this.pageKeys = [];
    this.currentPageKey = undefined;
    this.updateData();
  }

  prevPage() {
    const prevPageKey = (this.pageKeys.length > 0) ? this.pageKeys.pop() : undefined;
    this.currentPageKey = prevPageKey;
    this.updateData(prevPageKey);
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

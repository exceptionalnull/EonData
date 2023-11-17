import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { SendMessageModel } from './models/SendMessageModel';
import { MessageListModel } from './models/MessageListModel';
import { ContactMessageModel } from './models/ContactMessageModel';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly contactFormApiEndpoint = environment.apiUrl + '/contact';

  constructor(private http: HttpClient) { }

  submitContactForm(message: SendMessageModel) {
    return this.http.post(this.contactFormApiEndpoint, message);
  }

  getMessageCount(unread?: boolean): Observable<number> {
    const unreadParam = (unread != null) ? `?unread=${unread}` : '';
    return this.http.get<number>(`${this.contactFormApiEndpoint}/total${unreadParam}`);
  }

  getMessageList(unread?: boolean): Observable<MessageListModel[]> {
    const unreadParam = (unread != null) ? `?unread=${unread}` : '';
    return this.http.get<MessageListModel[]>(`${this.contactFormApiEndpoint}${unreadParam}`);
  }

  getMessage(messageId: string): Observable<ContactMessageModel | null> {
    return this.http.get<ContactMessageModel | null>(`${this.contactFormApiEndpoint}/message?id=${messageId}`);
  }

  setRead(messageId: string): Observable<object> {
    return this.http.put(`${this.contactFormApiEndpoint}/setread`, `id=${messageId}`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { SendMessageModel } from './models/SendMessageModel';
import { ContactMessageModel } from './models/ContactMessageModel';
import { MessageListModel } from './models/MessageListModel';

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
    const unreadParam = (unread !== undefined) ? `?unread=${unread}` : '';
    return this.http.get<number>(`${this.contactFormApiEndpoint}/total${unreadParam}`);
  }

  getMessageList(unread?: boolean, pagekey?: string): Observable<MessageListModel[]> {
    const params: string[] = [];
    if (unread !== undefined) {
      params.push(`unread=${unread}`);
    }
    if (pagekey !== undefined) {
      params.push(`startKey=${pagekey}`);
    }

    const urlParam = (params.length > 0) ? `?${params.join("&")}` : "";
    return this.http.get<MessageListModel[]>(`${this.contactFormApiEndpoint}${urlParam}`);
  }

  getMessage(messageId: string): Observable<ContactMessageModel | null> {
    return this.http.get<ContactMessageModel | null>(`${this.contactFormApiEndpoint}/${messageId}`);
  }

  setRead(messageId: string): Observable<object> {
    return this.http.put(`${this.contactFormApiEndpoint}/${messageId}/setread`, '');
  }
}

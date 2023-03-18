import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ContactMessageModel } from '../models/ContactMessageModel';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly contactFormApiEndpoint = environment.apiUrl + '/contact';

  constructor(private http: HttpClient) { }

  submitContactForm(message: ContactMessageModel) {
    console.log(environment.apiUrl);
    return this.http.post(this.contactFormApiEndpoint, message);
  }
}

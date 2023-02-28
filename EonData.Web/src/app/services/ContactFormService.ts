import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environment';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly contactUrl = environment.apiUrl + '/contact';

  constructor(private http: HttpClient) { }

  submitContactForm(email: string, message: string) {
    const formData = new FormData();
    formData.append('email', email);
    formData.append('message', message);
    return this.http.post(this.contactUrl + '/send', formData);
  }
}

export class ContactMessageModel {
  contactAddress?: string;
  message?: string;
  source?: string;

  constructor(partial: Partial<ContactMessageModel>) {
    Object.assign(this, partial);
  }
}

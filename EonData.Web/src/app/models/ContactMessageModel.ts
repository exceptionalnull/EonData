export class ContactMessageModel {
  name?: string;
  contactAddress?: string;
  message?: string;
  source?: string;
  timestamp: Date = new Date();

  constructor(partial?: Partial<ContactMessageModel>) {
    Object.assign(this, partial);
  }
}

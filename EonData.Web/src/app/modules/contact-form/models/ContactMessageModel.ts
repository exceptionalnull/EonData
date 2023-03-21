export class ContactMessageModel {
  messageId?: string;
  messageTimestamp: Date = new Date();
  contactName?: string;
  contactAddress?: string;
  messageContent?: string;
  formSource?: string;
  isRead: boolean = false;

  constructor(partial?: Partial<ContactMessageModel>) {
    Object.assign(this, partial);
  }
}

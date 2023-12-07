export class ContactMessageModel {
  messageId?: string;
  messageTimestamp?: Date;
  contactName?: string;
  contactAddress?: string;
  messageContent?: string;
  formSource?: string;
  requestSource?: string;
  isRead?: boolean;

  constructor(partial?: Partial<ContactMessageModel>) {
    Object.assign(this, partial);
  }
}

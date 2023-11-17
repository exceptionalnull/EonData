export class MessageListModel {
  messageId!: string;
  messageTimestamp!: Date;
  contactName!: string;
  contactAddress!: string;
  isRead!: boolean;
  constructor(partial?: Partial<MessageListModel>) {
    Object.assign(this, partial);
  }
}

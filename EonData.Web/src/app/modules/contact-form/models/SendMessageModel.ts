export class SendMessageModel {
  contactName: string = "";
  contactAddress: string = "";
  messageContent: string = "";
  formSource: string;

  constructor(source: string) {
    this.formSource = source;
  }
}

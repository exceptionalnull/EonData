import { MessageListModel } from "./MessageListModel";

export class MessageListResponseModel {
  public messages: MessageListModel[];
  public startKey?: string;

  constructor(results: MessageListModel[]) {
    this.messages = results;
  }
}

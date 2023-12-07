import { ShareFileModel } from "./ShareFileModel";

export class ShareFolderModel {
  name!: string;
  prefix!: string;
  files!: ShareFileModel[];
  lastModified?: Date | null;
}

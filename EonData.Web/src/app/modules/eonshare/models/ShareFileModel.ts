export class ShareFileModel {
  key!: string;
  name!: string;
  prefix!: string;
  size!: number;
  lastModified!: Date;

  get prettySize(): string {
    return this.humanReadableSize(this.size);
  }

  humanReadableSize(bytes: number): string {
    const units = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    let l = 0, n = bytes;

    while (n >= 1024 && ++l) {
      n = n / 1024;
    }

    // Include a maximum of 2 decimal places
    return (n.toFixed(n < 10 && l > 0 ? 2 : 0) + ' ' + units[l]);
  }
}

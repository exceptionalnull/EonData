import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { EonShareService } from '../../eonshare.service';
import { ShareFolderModel } from '../../models/ShareFolderModel';
import { ActivatedRoute, Router } from '@angular/router';
import { ShareFileModel } from '../../models/ShareFileModel';


@Component({
    selector: 'app-eonshare',
    templateUrl: './eonshare.component.html',
    styleUrls: ['./eonshare.component.scss'],
    standalone: false
})
export class EonShareComponent implements OnInit {
  public fileShare: ShareFolderModel[] = [];
  public isLoaded: boolean = false;
  public currentFolderKey: string = "";
  constructor(
    private fileService: EonShareService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) { }

  ngOnInit() {
    this.currentFolderKey = this.route.snapshot.paramMap.get('objectKey') ?? '';


    this.route.paramMap.subscribe(params => {
      this.currentFolderKey = params.get('objectKey') ?? '';
    });

    this.fileService.getFileShare().subscribe(response => {
      this.fileShare = response;
      this.isLoaded = true;
    });
  }

  getSubfolders(): string[] {
    return this.fileShare
      .filter(folder => folder.prefix.startsWith(this.currentFolderKey) && folder.prefix !== this.currentFolderKey && !folder.prefix.includes("/", this.currentFolderKey.length+1))
      .map(folder => folder.prefix.substring(folder.prefix.lastIndexOf("/") + 1));
  }

  getAllFolders(): string[] {
    return this.fileShare.map(folder => folder.prefix);
  }

  setFolder(folderKey: string) {
    if (this.currentFolderKey != "" && !this.fileShare.some(folder => folder.prefix === this.currentFolderKey)) {
      // folder doesn't exist so redirect to the base URL
      this.router.navigate(['fshare']);
    }
    else {
      //this.currentFolderKey = folderKey;
      this.router.navigate(['fshare', folderKey], { skipLocationChange: true }).then(() => {
        this.location.replaceState('fshare/' + folderKey);
      });
    }
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

  getFiles(): ShareFileModel[] {
    const folder = this.fileShare.find(f => f.prefix === this.currentFolderKey);
    return folder ? folder.files.filter(f => !f.name.endsWith("/")) : [];
  }

  getDownloadUrl(objectKey: string): string {
    return this.fileService.getDownloadUrl(objectKey);
  }
}

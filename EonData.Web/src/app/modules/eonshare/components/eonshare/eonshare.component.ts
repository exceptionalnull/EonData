import { Component, OnInit } from '@angular/core';
import { EonShareService } from '../../eonshare.service';
import { ShareFolderModel } from '../../models/ShareFolderModel';
import { ActivatedRoute, Router } from '@angular/router';
import { ShareFileModel } from '../../models/ShareFileModel';

@Component({
  selector: 'app-eonshare',
  templateUrl: './eonshare.component.html',
  styleUrls: ['./eonshare.component.scss']
})
export class EonshareComponent implements OnInit {
  public fileShare: ShareFolderModel[] = [];
  public currentFolderKey: string = '';
  constructor(private fileService: EonShareService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.currentFolderKey = this.route.snapshot.paramMap.get('objectKey') ?? '';
    this.fileService.getFileShare().subscribe(response => {
      this.fileShare = response;
      if (this.currentFolderKey != "" && !this.fileShare.some(folder => folder.prefix === this.currentFolderKey)) {
        // folder doesn't exist so redirect to the base URL
        this.router.navigate(['fshare']);
      }
    });
  }

  getSubfolders(): string[] {
    return this.fileShare
      .filter(folder => folder.prefix.startsWith(this.currentFolderKey) && folder.prefix !== this.currentFolderKey && !folder.prefix.includes("/", this.currentFolderKey.length))
      .map(folder => folder.prefix);
  }


  getFiles(): ShareFileModel[] {
    const folder = this.fileShare.find(f => f.prefix === this.currentFolderKey ?? '');
    return folder ? folder.files.filter(f => !f.name.endsWith("/")) : [];
  }

  getDownloadUrl(objectKey: string): string {
    return this.fileService.getDownloadUrl(objectKey);
  }
}

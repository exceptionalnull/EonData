import { Component, OnInit } from '@angular/core';
import { EonShareService } from '../../eonshare.service';
import { ShareFolderModel } from '../../models/ShareFolderModel';
import { ActivatedRoute } from '@angular/router';
import { ShareFileModel } from '../../models/ShareFileModel';

@Component({
  selector: 'app-eonshare',
  templateUrl: './eonshare.component.html',
  styleUrls: ['./eonshare.component.scss']
})
export class EonshareComponent implements OnInit {
  public fileShare: ShareFolderModel[] = [];
  public currentFolderKey: string = '';
  constructor(private fileService: EonShareService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.currentFolderKey = this.route.snapshot.paramMap.get('objectKey') ?? '';
    this.fileService.getFileShare().subscribe(response => {
      this.fileShare = response;
    });
  }

  getFiles(): ShareFileModel[] {
    const folder = this.fileShare.find(f => f.prefix === this.currentFolderKey ?? '');
    return folder ? folder.files : [];
  }
}

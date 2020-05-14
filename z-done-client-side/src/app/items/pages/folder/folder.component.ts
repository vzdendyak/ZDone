import {Component, Input, OnInit} from '@angular/core';
import {Folder} from '../../../models/folder';
import {List} from '../../../models/list';
import {FolderService} from '../../services/folder.service';

@Component({
  selector: 'app-folder',
  templateUrl: './folder.component.html',
  styleUrls: ['./folder.component.css']
})
export class FolderComponent implements OnInit {

  @Input() folder: Folder;
  lists: List[];
  showLists: boolean;

  constructor(private folderService: FolderService) {
    this.showLists = false;
  }

  ngOnInit() {
    this.folderService.getRelatedLists(this.folder.id).subscribe(value => {
      this.lists = value;
      console.log('Got lists for folder ' + this.folder.id + ' - ' + this.lists);
    });
  }

  showList() {
    this.showLists = !this.showLists;
  }

}

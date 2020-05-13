import {Component, OnInit} from '@angular/core';
import {ListService} from '../../services/list.service';
import {FolderService} from '../../services/folder.service';
import {List} from '../../../models/list';
import {Folder} from '../../../models/folder';

@Component({
  selector: 'app-menu-details',
  templateUrl: './menu-details.component.html',
  styleUrls: ['./menu-details.component.css']
})
export class MenuDetailsComponent implements OnInit {

  folders: Folder[];
  lists: List[];

  constructor(private listService: ListService, private folderService: FolderService) {
    this.folderService.getFolders().subscribe(value => {
      this.folders = value;
    });
    this.listService.getLists().subscribe(value => {
      this.lists = value;
    });

  }

  ngOnInit() {

  }


}

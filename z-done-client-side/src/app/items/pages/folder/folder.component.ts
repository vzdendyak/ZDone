import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Folder } from '../../../models/folder';
import { List } from '../../../models/list';
import { FolderService } from '../../services/folder.service';
import { MatMenuTrigger } from '@angular/material';
import { ListService } from '../../services/list.service';

@Component({
  selector: 'app-folder',
  templateUrl: './folder.component.html',
  styleUrls: ['./folder.component.css']
})
export class FolderComponent implements OnInit {

  @Input() folder: Folder;
  lists: List[];
  showLists: boolean;
  menuState: boolean;
  @Output() dialog = new EventEmitter<boolean>();

  @ViewChild(MatMenuTrigger, { static: false }) trigger: MatMenuTrigger;

  contextMenuPosition = { x: '0px', y: '0px' };

  constructor(private folderService: FolderService, private listService: ListService) {

  }

  ngOnInit() {
    setTimeout(() => {
      if (this.folder.id == this.folderService.folderIdToShow) {
        this.showLists = true;
      } else {
        this.showLists = false;
      }

    }, 10);
    this.folderService.getRelatedLists(this.folder.id).subscribe(value => {
      this.lists = value;
      console.log('Got lists for folder ' + this.folder.id + ' - ' + this.lists);
    });
  }

  showList() {
    this.showLists = !this.showLists;
  }

  rightClickFolder(event) {
    event.preventDefault();
    this.contextMenuPosition.x = event.clientX + 'px';
    this.contextMenuPosition.y = event.clientY + 'px';
    // this.trigger.menuData = { 'item': item };
    this.trigger.menu.focusFirstItem('mouse');
    this.trigger.openMenu();
    // this.trigger.menu.focusFirstItem();
  }


  addListClick() {
    this.folderService.openListDialogSubject.next(null);
  }

  addFolderClick() {
    this.folderService.openFolderDialogSubject.next(this.folder);
  }

  deleteFolderClick() {
    this.folderService.deleteFolderSubject.next(this.folder);
  }
}

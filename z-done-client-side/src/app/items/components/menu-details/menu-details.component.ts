import {Component, OnInit} from '@angular/core';
import {ListService} from '../../services/list.service';
import {FolderService} from '../../services/folder.service';
import {List} from '../../../models/list';
import {Folder} from '../../../models/folder';
import {MatDialog} from '@angular/material';
import {CreateListFormComponent} from '../create-list-form/create-list-form.component';

@Component({
  selector: 'app-menu-details',
  templateUrl: './menu-details.component.html',
  styleUrls: ['./menu-details.component.css']
})
export class MenuDetailsComponent implements OnInit {

  folders: Folder[];
  lists: List[];


  constructor(private listService: ListService, private folderService: FolderService, public dialog: MatDialog) {
    this.folderService.getFolders().subscribe(value => {
      this.folders = value;
    });
    this.listService.getLists().subscribe(value => {
      this.lists = value;
    });
    this.folderService.openListDialogSubject.subscribe(value => {
      this.openDialog(value);
    });
    this.folderService.deleteListSubject.subscribe(value => {
      this.deleteList(value);
    });

  }

  ngOnInit() {
  }

  openDialog(list: List) {
    let dialogRef;
    if (list == null) {
      dialogRef = this.dialog.open(CreateListFormComponent, {
        width: '450px'
      });
    } else {
      dialogRef = this.dialog.open(CreateListFormComponent, {
        width: '450px',
        data: {
          item: list
        }
      });
    }

    dialogRef.afterClosed().subscribe(value => {
      console.log('dialog closed');
      if (list == null) {
        if (value.folder != null) {
          let index = this.folders.findIndex(fldr => fldr.id == value.folder.id);
          this.folderService.folderIdToShow=value.folder.id;
          this.folders.splice(index, 1, value.folder);
        }
        this.lists.push(value.list);
      } else {
        if (value.folder != null) {
          let index = this.folders.findIndex(fldr => fldr.id == value.folder.id);
          this.folders.splice(index, 1, value.folder);
          index = this.lists.findIndex(l => l.id == list.id);
          this.folderService.folderIdToShow=list.folderId;
          this.lists.splice(index, 1, value.list);
        } else {
          let index = this.lists.findIndex(l => l.id == list.id);
          this.folderService.folderIdToShow=list.folderId;
          this.lists.splice(index, 1, value.list);
        }
      }
    });
  }

  deleteList(list: List) {
    this.listService.deleteList(list.id).subscribe(value => {
      let index = this.folders.findIndex(fldr => fldr.id == list.folderId);
      this.folders.splice(index, 1, list.folder);
      index = this.lists.findIndex(l => l.id == list.id);
      this.lists.splice(index, 1, list);
      this.folderService.folderIdToShow=list.folderId;
    });

  }
}
